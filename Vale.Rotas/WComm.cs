using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Rotas
{
    public class WComm
    {
        public virtual long Id { get; set; }
        public string message { get; set; }
        public long routeId { get; set; }
        public bool replace { get; set; } = false;
        public long replaceId { get; set; } = -1;
        public long tabindex { get; set; } = -1;
        public bool isOpen_Add { get; set; } = false;
        public bool isOpen_Consistency { get; set; } = false;
    }
}
