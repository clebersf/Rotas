using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vale.Rotas.Model;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Class;
using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Implementation.AssetManager.Interface;


namespace Vale.Rotas.ViewModel
{
    /// <summary>
    /// View model para interação com a interface GUI
    /// </summary>
    public class QueueViewModel : ObservableCollection<Queue_>
    {
        // Instancias de objetos da camada de infraestrutura para transações com o banco de dados
        private IrRouteQueueWriteRead rRouteQueueWriteRead { get; set; }
        private Ivw_Route_Consistency_Feeder_ReadOnly vw_Route_Consistency_Feeder_ReadOnly { get; set; }
        private Ivw_Route_Consistency_Reversal_ReadOnly vw_Route_Consistency_Reversal_ReadOnly { get; set; }
        private Ivw_Route_Consistency_Tripper_ReadOnly vw_Route_Consistency_Tripper_ReadOnly { get; set; }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public QueueViewModel()
        {
            this.rRouteQueueWriteRead = new rRouteQueueWriteRead();
            this.vw_Route_Consistency_Feeder_ReadOnly = new vw_Route_Consistency_Feeder_ReadOnly();
            this.vw_Route_Consistency_Reversal_ReadOnly = new vw_Route_Consistency_Reversal_ReadOnly();
            this.vw_Route_Consistency_Tripper_ReadOnly = new vw_Route_Consistency_Tripper_ReadOnly();
            PrepareCollection();
            
        }

        /// <summary>
        /// Consulta ao banco de dados para atualização da interface
        /// </summary>
        private void PrepareCollection()
        {
            var queue = from rot in this.rRouteQueueWriteRead.All()
                        select new Queue_
                        {
                            Id = rot.Location.Id,
                            Completa = rot.Location.Description,
                            Resumida = rot.Location.Alias,
                            Consistente = this.vw_Route_Consistency_Feeder_ReadOnly.All().Any(p => p.BoolVeredict == 0 && p.Id == rot.Id)||
                            this.vw_Route_Consistency_Reversal_ReadOnly.All().Any(p => p.BoolVeredict == 0 && p.Id == rot.Id) ||
                            this.vw_Route_Consistency_Tripper_ReadOnly.All().Any(p => p.BoolVeredict == 0 && p.Id == rot.Id)? "Não" : "Sim"
                        };
            foreach (var item in queue)
            {
                Add(item);
            }
        }
    }
}
