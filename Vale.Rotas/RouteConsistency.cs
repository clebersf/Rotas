using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Rotas
{
    internal class RouteConsistency
    {
        public virtual long Id { get; set; }
        public virtual bool Consistency { get; set; }
    }
}
