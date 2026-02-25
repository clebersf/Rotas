using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Rotas.Model
{
    /// <summary>
    /// Objeto de fila de rota com os atributos das rotas que compõe a fila de rotas com herança INotifyPropertyChanged para provocar um evento mediante uma mudança para alteração da interface GUI
    /// </summary>
    public class Queue_ : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Evento de mudança de objeto
        /// </summary>
        private void OnPropertyChanged(string Propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Propertyname));
        }
        // Id da rota
        private long _id { get; set; }

        //String completa da rota
        private string _completa { get; set; }
        // String rota resumida
        private string _resumida { get; set; }
        // Rota que irá substituir a rota atual (em caso de substituição de rotas)
        private string _consistente { get; set; }
        // Id da rota
        public long Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        //String completa da rota
        public string Completa
        {
            get { return _completa; }
            set
            {
                _completa = value;
                OnPropertyChanged("Completa");
            }
        }
        // String rota resumida
        public string Resumida
        {
            get { return _resumida; }
            set
            {
                _resumida = value;
                OnPropertyChanged("Resumida");
            }
        }
        // Rota que irá substituir a rota atual (em caso de substituição de rotas)
        public string Consistente
        {
            get { return _consistente; }
            set
            {
                _consistente = value;
                OnPropertyChanged("Consistente");
            }
        }
    }
}