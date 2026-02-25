using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vale.Tops.Domain
{
    public class vwUserRule
    {
        /// <summary>
        ///  Matricula
        /// </summary>
        public string ValeId { get; set; }

        /// <summary>
        ///  Nome do Uauário
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///  Conta do domínio
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        ///  Descrição do localInstalacao
        /// </summary>
        public string DescriptionRole { get; set; }

        /// <summary>
        ///  Descrição do localInstalacao
        /// </summary>
        public string DescriptionApplication { get; set; }

        /// <summary>
        ///  Descrição do localInstalacao
        /// </summary>
        public string GroupDns { get; set; }

        /// <summary>
        ///  Verificação se o usuario é realmente membro no domínio
        /// </summary>
        public bool IsMember { get; set; }

    }
}
