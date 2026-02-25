// Global.asax.cs

using System.Reflection; // Para registrar Controllers
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

// Seus namespaces (ajuste conforme a estrutura real dos seus projetos)
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source; // Para IReadOnly<T>
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager;
// (Você pode remover 'Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager'
// se ele não for usado diretamente, mas não causa problema se for mantido.)


namespace Vale.Tops.Application.Presentation.Diagnosis
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // 1. Configuração padrão do MVC
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // ------------------------------------------------------------------
            // 2. CONFIGURAÇÃO DA INJEÇÃO DE DEPENDÊNCIA (AUTOFAC)
            // ------------------------------------------------------------------

            // 1. Crie o ContainerBuilder
            var builder = new ContainerBuilder();

            // 2. Registre seus Controllers
            // Isso registra todos os Controllers do Assembly atual, incluindo o ConsistencyController
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Se o seu ConsistencyController estiver em outro projeto/Assembly, use:
            // builder.RegisterControllers(typeof(Vale.Tops.Application.Presentation.Diagnosis.Controllers.ConsistencyController).Assembly);

            // 3. Carregue seu Módulo de Repositórios (Este é o passo que resolve as dependências)
            builder.RegisterModule(new RepositoryModule());

            // Opcional: Registre o filtro global (se usar injeção em Filters)
            // builder.RegisterFilterProvider();

            

            // B. Registro de Repositórios ReadOnly (Interfaces -> Classes Concretas)

            // 1. REGISTRO GENÉRICO: Solução para IReadOnly<rRouteActive> e IReadOnly<rRouteQueue>
            builder.RegisterGeneric(typeof(ReadOnly<>))
                   .As(typeof(IReadOnly<>))
                   .InstancePerRequest();

            // 2. Registro Específico: Para a view de consistência
            builder.RegisterType<vw_Route_Consistency_ReadOnly>()
                   .As<Ivw_Route_Consistency_ReadOnly>()
                   .InstancePerRequest();

            // 3. Registro de outros Repositórios (Mantenha o padrão, se aplicável)
            // Ex: builder.RegisterType<rRouteQueueReadOnly>().As<IrRouteQueueReadOnly>().InstancePerRequest();


            // C. Finalizar e Definir o Resolvedor de Dependência do MVC
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}