using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Tops.Domain
{
    public class GroupDns
    {
        /// <summary>
        ///  Descrição do localInstalacao
        /// </summary>
        [Key]
        public virtual long Id { get; set; }
        public string name { get; set; }


    }
}
