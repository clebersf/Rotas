using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Tops.Domain
{
    public class vwCockpitCentreon 
    {
        public object last_check { get; set; }
        public object last_state_change { get; set; }
        public object last_hard_state_change { get; set; }
        public object id { get; set; }
        public object Name { get; set; }
        public object alias { get; set; }
        public object address { get; set; }
        public object state { get; set; }
        public object state_type { get; set; }
        public object output { get; set; }

    }
}
