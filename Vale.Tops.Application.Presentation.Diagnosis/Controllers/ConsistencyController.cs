// Local: Vale.Tops.Application.Presentation.Diagnosis/Controllers/ConsistencyController.cs

using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using Vale.Tops.Application.Presentation.Diagnosis.ViewModels;
using System;
// Importe os namespaces das suas interfaces ReadOnly (Ajuste conforme a localização real):
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;
using Vale.Tops.Domain;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source;
using Vale.Tops.Web.Mvc.ViewModels; // Para usar rRouteActive e rRouteQueue (assumindo que estão lá)

namespace Vale.Tops.Application.Presentation.Diagnosis.Controllers
{
    public class ConsistencyController : Controller
    {
        // 🚨 Novas Injeções de Repositórios ReadOnly
        private readonly Ivw_Route_Consistency_ReadOnly _routeConsReadOnly; // Existente
        private readonly Ivw_Route_Consistency_Damper_ReadOnly _damperConsReadOnly;
        private readonly Ivw_Route_Consistency_Feeder_ReadOnly _feederConsReadOnly;
        private readonly Ivw_Route_Consistency_Reversal_ReadOnly _reversalConsReadOnly;
        private readonly Ivw_Route_Consistency_Rule_ReadOnly _ruleConsReadOnly;
        private readonly Ivw_Route_Consistency_Tripper_ReadOnly _tripperConsReadOnly;

        // Injeção dos Repositórios (Declaração de Campos Privados e Construtor)
        private readonly Ivw_Route_Tag_Damper_Permission_ReadOnly _permissionDamperReadOnly;
        private readonly Ivw_Route_Tag_Feeder_Permission_ReadOnly _permissionFeederReadOnly;
        private readonly Ivw_Route_Tag_Reversal_Permission_ReadOnly _permissionReversalReadOnly;
        private readonly Ivw_Route_Tag_Tripper_Permission_ReadOnly _permissionTripperReadOnly;

        // Injeção dos Repositórios (Declaração de Campos Privados e Construtor)
        private readonly Ivw_Route_Tag_Damper_Command_ReadOnly _CommandDamperReadOnly;
        private readonly Ivw_Route_Tag_Feeder_Command_ReadOnly _CommandFeederReadOnly;
        private readonly Ivw_Route_Tag_Reversal_Command_ReadOnly _CommandReversalReadOnly;
        private readonly Ivw_Route_Tag_Tripper_Command_ReadOnly _CommandTripperReadOnly;

        // Declaração dos Repositórios ReadOnly (Injetados pelo Autofac)
        private readonly Ivw_Route_Consistency_ReadOnly _consReadOnly;
        private readonly IWriteRead<rRouteActive> _activeReadOnly; // Usando a interface genérica
        private readonly IWriteRead<rRouteQueue> _queueReadOnly;   // Usando a interface genérica
                                                                  // Exemplo de Injeção
                                                                  // Injetando o Repositório de Leitura para a entidade principal (Location)
                                                                  // 🚨 Repositório de Leitura para a Tabela Location 🚨
                                                                  // Usamos IReadOnly, pois é uma operação de CONSULTA complexa com JOINs (Include),
                                                                  // e o IReadOnly utiliza o ReadOnlyContext, que é mais adequado para relatórios.
        private readonly IWriteRead<Location> _locationReadOnlyRepository;

        // Construtor para Injeção de Dependência (O Autofac usa este construtor)
        public ConsistencyController(
            Ivw_Route_Consistency_ReadOnly consReadOnly,
            IWriteRead<rRouteActive> activeReadOnly,
            IWriteRead<rRouteQueue> queueReadOnly,
            IWriteRead<Location> locationReadOnlyRepository,
            Ivw_Route_Consistency_ReadOnly routeConsReadOnly,
    Ivw_Route_Consistency_Damper_ReadOnly damperConsReadOnly,
    Ivw_Route_Consistency_Feeder_ReadOnly feederConsReadOnly,
    Ivw_Route_Consistency_Reversal_ReadOnly reversalConsReadOnly,
    Ivw_Route_Consistency_Rule_ReadOnly ruleConsReadOnly,
    Ivw_Route_Consistency_Tripper_ReadOnly tripperConsReadOnly,
    Ivw_Route_Tag_Damper_Permission_ReadOnly permissionDamperReadOnly,
            Ivw_Route_Tag_Feeder_Permission_ReadOnly permissionFeederReadOnly,
            Ivw_Route_Tag_Reversal_Permission_ReadOnly permissionReversalReadOnly,
            Ivw_Route_Tag_Tripper_Permission_ReadOnly permissionTripperReadOnly,
    Ivw_Route_Tag_Damper_Command_ReadOnly CommandDamperReadOnly,
            Ivw_Route_Tag_Feeder_Command_ReadOnly CommandFeederReadOnly,
            Ivw_Route_Tag_Reversal_Command_ReadOnly CommandReversalReadOnly,
            Ivw_Route_Tag_Tripper_Command_ReadOnly CommandTripperReadOnly)
        {
            _routeConsReadOnly = routeConsReadOnly;
            _damperConsReadOnly = damperConsReadOnly;
            _feederConsReadOnly = feederConsReadOnly;
            _reversalConsReadOnly = reversalConsReadOnly;
            _ruleConsReadOnly = ruleConsReadOnly;
            _tripperConsReadOnly = tripperConsReadOnly;
            _consReadOnly = consReadOnly;
            _activeReadOnly = activeReadOnly;
            _queueReadOnly = queueReadOnly;
            _locationReadOnlyRepository = locationReadOnlyRepository;
            _permissionDamperReadOnly = permissionDamperReadOnly;
            _permissionFeederReadOnly = permissionFeederReadOnly;
            _permissionReversalReadOnly = permissionReversalReadOnly;
            _permissionTripperReadOnly = permissionTripperReadOnly;
            _CommandDamperReadOnly = CommandDamperReadOnly;
            _CommandFeederReadOnly = CommandFeederReadOnly;
            _CommandReversalReadOnly = CommandReversalReadOnly;
            _CommandTripperReadOnly = CommandTripperReadOnly;
        }

        // GET: /Consistency/Index
        public ActionResult Index()
        {
            // 1. Obter todas as consistências da View SQL (vw_Route_Consistency)
            // Converte para Dicionário para lookup rápido (Id -> Consistency Status)
            var consistencies = _consReadOnly.All().ToDictionary(c => c.Id, c => c.Consistency == 1);

            // 2. Processar Rotas na FILA (rRouteQueue)
            var queueRoutes = _queueReadOnly.All()
                .Select(rot => new RouteConsistencyViewModel
                {
                    RouteId = rot.Id,
                    NomeCompleto = rot.Location.Name, // Acesso à navegação Location
                    NomeResumido = rot.Location.Alias,
                    TipoRota = "Fila",
                    Consistente = consistencies.ContainsKey(rot.Id) ? consistencies[rot.Id] : false,
                    UltimaAtualizacao = DateTime.Now, // Substituir por rot.dh se disponível
                    // Buscar detalhe SE Inconsistente (Consistente = false)
                    DetalhesInconsistencia = (consistencies.ContainsKey(rot.Id) && !consistencies[rot.Id]) ? GetConsistencyDetails(rot.Id) : new List<RouteConsistencyDetailViewModel>()
                });

            // 3. Processar Rotas ATIVAS (rRouteActive)
            var activeRoutes = _activeReadOnly.All()
                .Select(rot => new RouteConsistencyViewModel
                {
                    RouteId = rot.Id,
                    NomeCompleto = rot.Location.Name, // Acesso à navegação Location
                    NomeResumido = rot.Location.Alias,
                    TipoRota = "Ativa",
                    Consistente = consistencies.ContainsKey(rot.Id) ? consistencies[rot.Id] : false,
                    UltimaAtualizacao = rot.dh, // Assumindo que rRouteActive tem campo dh
                    DetalhesInconsistencia = (consistencies.ContainsKey(rot.Id) && !consistencies[rot.Id]) ? GetConsistencyDetails(rot.Id) : new List<RouteConsistencyDetailViewModel>()
                });

            // 4. Combina, prioriza Ativas e ordena inconsistentes no topo
            var allRoutes = activeRoutes.Union(queueRoutes)
                                        .OrderByDescending(r => r.TipoRota == "Ativa")
                                        .ThenBy(r => r.Consistente) // Inconsistente (false) vem antes de Consistente (true)
                                        .ToList();

            return View(allRoutes);
        }

        // MOCK/Placeholder para a lógica de Detalhe da Falha (CRÍTICO para a Manutenção)
        // ESTE MÉTODO PRECISA DA IMPLEMENTAÇÃO DA VIEW SQL vw_Route_Consistency_Detail
        private List<RouteConsistencyDetailViewModel> GetConsistencyDetails(long routeId)
        {
            // Retorno Mockado para rotas inconsistentes (A ser substituído pela chamada ao Ivw_Route_Consistency_Detail_ReadOnly)
            return new List<RouteConsistencyDetailViewModel>
            {
                new RouteConsistencyDetailViewModel { Equipamento = "C_101", MensagemDeErro = "Regra de Limite Ativo: Rota excede o máximo permitido na correia." },
                new RouteConsistencyDetailViewModel { Equipamento = "VIBRADOR_5", MensagemDeErro = "Consistência de Damper: Posição Divergente da Rota (Esperado: Fechada)." },
            };
        }

        // ----------------------------------------------------------------------
        // NOVA ACTION: Rotas Ativas
        // ----------------------------------------------------------------------

        /// <summary>
        /// Obtém e exibe a lista de todas as rotas ativas usando LINQ no repositório genérico.
        /// </summary>
        public ActionResult ActiveRoutes()
        {
            try
            {
                // A query mais importante:
                // 1. Usa o método Find que aceita o parâmetro 'include' por string.
                // 2. Inclui os relacionamentos rRouteActive e Plc (para pegar o nome/resource).
                // 3. Filtra: l.rRouteActive != null (apenas rotas que possuem um registro na tabela de rotas ativas).

                var activeLocations = _locationReadOnlyRepository.Find(
                    where: l => l.rRouteActive != null
                    //,include: "rRouteActive,Plc" // Inclui a relação de ativação e a relação com Plc para o nome
                ).AsQueryable();

                // Projeta o resultado para o ActiveRouteViewModel (DTO)
                var viewModelList = activeLocations
                    .Select(l => new ActiveRouteViewModel
                    {
                        Id = l.Id,
                        // Assumindo que o nome descritivo da rota está em Plc.Resource
                        Name = l.Name,
                        ActivationDate = l.rRouteActive.dh
                    })
                    .ToList();

                return View(viewModelList);
            }
            catch (System.Exception ex)
            {
                var exMsg = ex.InnerException;
                // Logar o erro (implementação omitida)
                // Retornar uma View de Erro ou uma lista vazia
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar as rotas ativas: " + ex.Message;
                return View(new List<ActiveRouteViewModel>());
            }
        }

        // Dentro de ConsistencyController.cs

        // Dentro de ConsistencyController.cs

        public ActionResult ConsolidatedConsistencyReport()
        {
            // O tipo de modelo que a View requer
            //using ConsistencyReportItemViewModel = Vale.Tops.Application.Presentation.Diagnosis.ViewModels.ConsistencyReportItem;

            // A variável 'report' é explicitamente declarada usando o ViewModel CORRETO.
            var report = new List<ConsistencyReportItemViewModel>();

            // Variável para armazenar o nome do equipamento consultado
            string equipmentName = string.Empty;

            try
            {
                // 1. Dicionário de Nomes de Equipamentos para Lookup Rápido
                var locationNames = _locationReadOnlyRepository.All()
                    .ToDictionary(l => l.Id, l => l.Name);

                // --- 2. Consulta de cada View de Consistência (Filtro CORRIGIDO para Consistency == 0) ---

                // 2.1. Rota/Geral (vw_Route_Consistency)
                var routeCons = _routeConsReadOnly.All()
                    .Where(x => x.Consistency == 0) // 🚨 CORREÇÃO DE LÓGICA: 0 é Inconsistente (conforme script.sql)
                    .Select(x => {
                        locationNames.TryGetValue(x.Id, out equipmentName);

                        // CORREÇÃO DE TIPO: Instanciando o ViewModel
                        return new ConsistencyReportItemViewModel
                        {
                            Id = x.Id,
                            ConsistencyValue = x.Consistency,
                            InconsistencyType = "Geral/Sequência",
                            EquipmentName = equipmentName ?? $"ID Desconhecido ({x.Id})"
                        };
                    });
                report.AddRange(routeCons.ToList()); // Adicionado .ToList() para evitar execuções de consulta múltiplas

                // 2.2. Damper (vw_Route_Consistency_Damper)
                var damperCons = _damperConsReadOnly.All()
                    .Where(x => x.BoolVeredict == 0) // 🚨 CORREÇÃO DE LÓGICA: 0 é Inconsistente
                    .Select(x => {
                        locationNames.TryGetValue(x.Id, out equipmentName);

                        // CORREÇÃO DE TIPO: Instanciando o ViewModel
                        return new ConsistencyReportItemViewModel
                        {
                            Id = x.Id,
                            ConsistencyValue = x.BoolVeredict,
                            InconsistencyType = "Damper",
                            EquipmentName = equipmentName ?? $"ID Desconhecido ({x.Id})"
                        };
                    });
                report.AddRange(damperCons.ToList());

                // 2.3. Feeder (vw_Route_Consistency_Feeder)
                var feederCons = _feederConsReadOnly.All()
                    .Where(x => x.BoolVeredict == 0) // 🚨 CORREÇÃO DE LÓGICA: 0 é Inconsistente
                    .Select(x => {
                        locationNames.TryGetValue(x.Id, out equipmentName);

                        // CORREÇÃO DE TIPO: Instanciando o ViewModel
                        return new ConsistencyReportItemViewModel
                        {
                            Id = x.Id,
                            ConsistencyValue = x.BoolVeredict,
                            InconsistencyType = "Feeder",
                            EquipmentName = equipmentName ?? $"ID Desconhecido ({x.Id})"
                        };
                    });
                report.AddRange(feederCons.ToList());

                // 2.4. Reversal (vw_Route_Consistency_Reversal)
                var reversalCons = _reversalConsReadOnly.All()
                    .Where(x => x.BoolVeredict == 0) // 🚨 CORREÇÃO DE LÓGICA: 0 é Inconsistente
                    .Select(x => {
                        locationNames.TryGetValue(x.Id, out equipmentName);

                        // CORREÇÃO DE TIPO: Instanciando o ViewModel
                        return new ConsistencyReportItemViewModel
                        {
                            Id = x.Id,
                            ConsistencyValue = x.BoolVeredict,
                            InconsistencyType = "Reversal",
                            EquipmentName = equipmentName ?? $"ID Desconhecido ({x.Id})"
                        };
                    });
                report.AddRange(reversalCons.ToList());

                // 2.5. Rule (vw_Route_Consistency_Rule)
                var ruleCons = _ruleConsReadOnly.All()
                    .Where(x => x.BoolVeredict == 0) // 🚨 CORREÇÃO DE LÓGICA: 0 é Inconsistente
                    .Select(x => {
                        locationNames.TryGetValue(x.Id, out equipmentName);

                        // CORREÇÃO DE TIPO: Instanciando o ViewModel
                        return new ConsistencyReportItemViewModel
                        {
                            Id = x.Id,
                            ConsistencyValue = x.BoolVeredict,
                            InconsistencyType = "Regra de Negócio",
                            EquipmentName = equipmentName ?? $"ID Desconhecido ({x.Id})"
                        };
                    });
                report.AddRange(ruleCons.ToList());

                // 2.6. Tripper (vw_Route_Consistency_Tripper)
                var tripperCons = _tripperConsReadOnly.All()
                    .Where(x => x.BoolVeredict == 0) // 🚨 CORREÇÃO DE LÓGICA: 0 é Inconsistente
                    .Select(x => {
                        locationNames.TryGetValue(x.Id, out equipmentName);

                        // CORREÇÃO DE TIPO: Instanciando o ViewModel
                        return new ConsistencyReportItemViewModel
                        {
                            Id = x.Id,
                            ConsistencyValue = x.BoolVeredict,
                            InconsistencyType = "Tripper",
                            EquipmentName = equipmentName ?? $"ID Desconhecido ({x.Id})"
                        };
                    });
                report.AddRange(tripperCons.ToList());

                // 3. Ordenação Final
                var finalReport = report
                    .OrderBy(x => x.EquipmentName)
                    .ThenBy(x => x.InconsistencyType)
                    .ToList();

                return View(finalReport);
            }
            catch (Exception ex)
            {
                // Tratamento de erro
                ViewBag.ErrorMessage = $"Erro ao gerar o relatório consolidado de consistência: {ex.Message}";
                // CORREÇÃO DE TIPO: Retornando uma lista vazia do ViewModel
                return View(new List<ConsistencyReportItemViewModel>());
            }
        }

        // Local: Vale.Tops.Application.Presentation.Diagnosis/Controllers/ConsistencyController.cs (Adição)

        // OBS: Assuma que as interfaces de repositório ReadOnly para todas as Views (Damper, Feeder, 
        // Reversal, Tripper) foram injetadas no construtor do Controller.

        public ActionResult AllEquipmentConsistency()
        {
            var allInconsistencies = new List<RouteEquipmentConsistencyViewModel>();

            try
            {
                // 1. Processar Inconsistências de DAMPER
                var damperCons = _damperConsReadOnly.All().Select(c => new RouteEquipmentConsistencyViewModel
                {
                    InconsistencyType = "DAMPER",
                    RouteId = c.Id,
                    RouteName = c.Route,
                    AssetCurrentName = c.AssetCurrent,
                    AssetCurrentId = c.AssetCurrentId,
                    AssetNextName = c.AssetNext,
                    AssetNextId = c.AssetNextId,
                    VeredictDescription = c.Veredict,
                    ReferenceId = c.ReferenceId,
                    TagName = c.TagName,
                    TagValue = ""
                });
                allInconsistencies.AddRange(damperCons);

                // 2. Processar Inconsistências de FEEDER
                var feederCons = _feederConsReadOnly.All().Select(c => new RouteEquipmentConsistencyViewModel
                {
                    InconsistencyType = "FEEDER",
                    RouteId = c.Id,
                    RouteName = c.Route,
                    AssetCurrentName = c.AssetCurrent,
                    AssetCurrentId = c.AssetCurrentId,
                    AssetNextName = c.AssetNext,
                    AssetNextId = c.AssetNextId,
                    VeredictDescription = c.Veredict,
                    ReferenceId = c.ReferenceId,
                    TagName = c.TagName,
                    TagValue = ""
                });
                allInconsistencies.AddRange(feederCons);

                // 3. Processar Inconsistências de REVERSAL
                var reversalCons = _reversalConsReadOnly.All().Select(c => new RouteEquipmentConsistencyViewModel
                {
                    InconsistencyType = "REVERSAL",
                    RouteId = c.Id,
                    RouteName = c.Route,
                    AssetCurrentName = c.AssetCurrent,
                    AssetCurrentId = c.AssetCurrentId,
                    AssetNextName = c.AssetNext,
                    AssetNextId = c.AssetNextId,
                    VeredictDescription = c.Veredict,
                    ReferenceId = c.ReferenceId,
                    TagName = c.TagName,
                    TagValue = ""
                });
                allInconsistencies.AddRange(reversalCons);

                // 4. Processar Inconsistências de TRIPPER
                var tripperCons = _tripperConsReadOnly.All().Select(c => new RouteEquipmentConsistencyViewModel
                {
                    InconsistencyType = "TRIPPER",
                    RouteId = c.Id,
                    RouteName = c.Route,
                    AssetCurrentName = c.AssetCurrent,
                    AssetCurrentId = c.AssetCurrentId,
                    AssetNextName = c.AssetNext,
                    AssetNextId = c.AssetNextId,
                    VeredictDescription = c.Veredict,
                    ModeCode = c.Mode, // Atributo específico do Tripper
                    DescriptionMode = c.DescriptionMode, // Atributo específico do Tripper
                    TagName = c.TagName, // Tripper também possui TagName
                    TripperCurrentValue = c.Current // Atributo específico do Tripper
                });
                allInconsistencies.AddRange(tripperCons);

                // Ordenar por Rota e Equipamento para melhor visualização
                var sortedList = allInconsistencies
                    .OrderBy(c => c.RouteName)
                    .ThenBy(c => c.AssetCurrentName)
                    .ToList();

                return View("AllEquipmentConsistency", sortedList);
            }
            catch (System.Exception ex)
            {
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar todas as inconsistências por equipamento: " + ex.Message;
                return View("AllEquipmentConsistency", new List<RouteEquipmentConsistencyViewModel>());
            }
        }


        // Nova Action
        public ActionResult AllTagPermissions()
        {
            var consolidatedList = new List<RouteTagPermissionViewModel>();

            // Função de mapeamento auxiliar
            Func<dynamic, string, RouteTagPermissionViewModel> mapToViewModel = (view, type) => new RouteTagPermissionViewModel
            {
                Id = view.Id,
                Route = view.Route,
                EquipmentType = type,
                Asset = view.Asset,
                TagName = view.TagName,
                PermissionStatus = view.PermissionVeredict,
                IsPermitted = view.BoolPermission == 1
            };

            try
            {
                // 1. Obter e Mapear Damper
                consolidatedList.AddRange(_permissionDamperReadOnly.All()
                    .Select(d => mapToViewModel(d, "DAMPER")));

                // 2. Obter e Mapear Feeder
                consolidatedList.AddRange(_permissionFeederReadOnly.All()
                    .Select(f => mapToViewModel(f, "FEEDER")));

                // 3. Obter e Mapear Reversal
                consolidatedList.AddRange(_permissionReversalReadOnly.All()
                    .Select(r => mapToViewModel(r, "REVERSAL")));

                // 4. Obter e Mapear Tripper
                consolidatedList.AddRange(_permissionTripperReadOnly.All()
                    .Select(t => mapToViewModel(t, "TRIPPER")));
            }
            catch (Exception ex)
            {
                // Tratamento de erro ou log
                ViewBag.ErrorMessage = $"Ocorreu um erro ao carregar as permissões: {ex.Message}";
                return View(new List<RouteTagPermissionViewModel>());
            }

            // Ordenar por Rota e Tipo de Equipamento
            var sortedList = consolidatedList.OrderBy(x => x.Route).ThenBy(x => x.EquipmentType).ToList();

            return View(sortedList);
        }

        // Nova Action
        public ActionResult AllTagCommands()
        {
            var consolidatedList = new List<RouteTagCommandViewModel>();

            // Função de mapeamento auxiliar
            Func<dynamic, string, RouteTagCommandViewModel> mapToViewModel = (view, type) => new RouteTagCommandViewModel
            {
                Id = view.Id,
                Route = view.Route,
                EquipmentType = type,
                Asset = view.Asset,
                TagName = view.TagName,
                CommandStatus = view.CommandVeredict,
                IsPermitted = view.BoolCommand == 1
            };

            try
            {
                // 1. Obter e Mapear Damper
                consolidatedList.AddRange(_CommandDamperReadOnly.All()
                    .Select(d => mapToViewModel(d, "DAMPER")));

                // 2. Obter e Mapear Feeder
                consolidatedList.AddRange(_CommandFeederReadOnly.All()
                    .Select(f => mapToViewModel(f, "FEEDER")));

                // 3. Obter e Mapear Reversal
                consolidatedList.AddRange(_CommandReversalReadOnly.All()
                    .Select(r => mapToViewModel(r, "REVERSAL")));

                // 4. Obter e Mapear Tripper
                consolidatedList.AddRange(_CommandTripperReadOnly.All()
                    .Select(t => mapToViewModel(t, "TRIPPER")));
            }
            catch (Exception ex)
            {
                // Tratamento de erro ou log
                ViewBag.ErrorMessage = $"Ocorreu um erro ao carregar as permissões: {ex.Message}";
                return View(new List<RouteTagCommandViewModel>());
            }

            // Ordenar por Rota e Tipo de Equipamento
            var sortedList = consolidatedList.OrderBy(x => x.Route).ThenBy(x => x.EquipmentType).ToList();

            return View(sortedList);
        }


    }
    // Exemplo de DTO para a tela
    // Exemplo de DTO: ConsistencyReportItem.cs

    public class ConsistencyReportItem
    {
        // ID da Rota ou Location que está inconsistente (chave primária da View de origem)
        public long Id { get; set; }

        // Nome do Equipamento ou Rota, obtido da tabela Location
        public string EquipmentName { get; set; }

        // O tipo de inconsistência, para agrupamento e filtragem
        public string InconsistencyType { get; set; }

        // O valor da inconsistência (0 = Inconsistente, 1 = Consistente)
        public int ConsistencyValue { get; set; }

        // Adicione outros campos de detalhe se as Views os fornecerem
        // public string DetailMessage { get; set; } 


    }
}