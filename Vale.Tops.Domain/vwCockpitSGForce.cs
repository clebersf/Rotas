using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Tops.Domain
{
    public class vwCockpitSGForce : Entity
    {
        [Key]
        public virtual Guid Id { get; set; }
        public Int64 idForce { get; set; }
       public DateTime  DataForceReal { get; set; }
       public string  Mat_Executante { get; set; }
       public string  Nome_Executante { get; set; }
       public string  ChaveRede { get; set; }
       public string  Endereco { get; set; }
       public string  CPU { get; set; }
       public string  Supervisao { get; set; }
       public int?  Criticidade { get; set; }
       public DateTime?  DataRemocao { get; set; }
    }
}
