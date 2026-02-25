namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rLogbookVV")]
    public partial class rLogbookVV : Entity
    {
        [Key]
        public virtual Guid Id { get; set; }
        public virtual string In_Diario_NomeOperador1 { get; set; }
        public virtual string In_Diario_NomeOperador2 { get; set; }
        public virtual string In_Diario_Observacao { get; set; }
        public virtual string In_Diario_ObservacaoTaxa { get; set; }
        public virtual bool In_Diario_VVRestrito { get; set; }
        public virtual bool In_Diario_RotaRestrita { get; set; }
        public virtual bool In_Diario_QualidadeMaterial { get; set; }
        public virtual int In_Diario_PartirCicloHora { get; set; }
        public virtual int In_Diario_PartirCicloMinuto { get; set; }
        public virtual int In_Diario_TerminoDescargaHora { get; set; }
        public virtual int In_Diario_TerminoDescargaMinuto { get; set; }
        public virtual int In_Diario_BalizaMenor { get; set; }
        public virtual int In_Diario_BalizaMaior { get; set; }
        public virtual int In_Diario_QuantidadeVagoes { get; set; }
        public virtual string In_Diario_Prefixo { get; set; }
        public virtual string In_Diario_Material { get; set; }
        public virtual string In_Diario_Area { get; set; }       
        public virtual string In_Diario_Destino { get; set; }
        public virtual int In_Diario_Operador1Hora { get; set; }
        public virtual int In_Diario_Operador1Minuto { get; set; }
        public virtual int In_Diario_Operador2Hora { get; set; }
        public virtual int In_Diario_Operador2Minuto { get; set; }
        public virtual int In_Diario_Operador1QtVagoes { get; set; }

        public virtual Logbook Logbook { get; set; }
    }
}
