using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Tops.Domain
{
    [Table("vwFTACChange")]
    public class vwFTACChange : Entity
    {
        [Key]
        public DateTime DateTimeOccurred { get; set; }
        public string Resource { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
    }
}
