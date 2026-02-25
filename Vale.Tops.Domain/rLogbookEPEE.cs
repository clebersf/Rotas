namespace  Vale.Tops.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("rLogbookEPEE")]
    public partial class rLogbookEPEE : Entity
    {
        [Key]
        public virtual Guid Id { get; set; }
        public virtual string In_Diario_Origem1 { get; set; }
        public virtual string In_Diario_Matricula { get; set; }
        public virtual string In_Diario_Material1 { get; set; }
        public virtual bool In_Diario_Fechamento_de_Pilha { get; set; }
        public virtual int In_Diario_Numero_Lote { get; set; }
        public virtual int In_Diario_Quantidade_Vagoes1 { get; set; }
        public virtual string In_Diario_Prefixo_Lote1 { get; set; }
        public virtual int In_Diario_Passadas { get; set; }
        public virtual int In_Diario_Baliza_Inicial { get; set; }
        public virtual int In_Diario_Baliza_Final { get; set; }
        public virtual int In_Diario_Baliza_Inicial_Lote { get; set; }
        public virtual int In_Diario_Baliza_Final_Lote { get; set; }
        public virtual int In_Diario_Hora_Inicial1 { get; set; }
        public virtual int In_Diario_Minuto_Inicial1 { get; set; }
        public virtual int In_Diario_Hora_Final1 { get; set; }
        public virtual int In_Diario_Minuto_Final1 { get; set; }
        public virtual bool In_Diario_Paralisacao { get; set; }
        public virtual string In_Diario_Observacao_Paralisacao { get; set; }
        public virtual string In_Diario_Observacao_Taxa { get; set; }
        public virtual bool In_Diario_Tremonha { get; set; }
        public virtual bool In_Diario_Qualidade_Material_Ok { get; set; }
        public virtual bool In_Diario_Rota_Restrita_Ok { get; set; }
        public virtual string In_Diario_Codigo_Operacao { get; set; }
        public virtual string In_Diario_Area { get; set; }
        public virtual string In_Diario_Empilhamento { get; set; }
        public virtual string In_Diario_Insumo { get; set; }
        public virtual int In_Diario_Hora_Comunicacao_P0 { get; set; }
        public virtual int In_Diario_Minuto_Comunicacao_P0 { get; set; }
        public virtual int In_Diario_Hora_Acoplado { get; set; }
        public virtual int In_Diario_Minuto_Acoplado { get; set; }
        public virtual int In_Diario_Hora_Rodou_Usina { get; set; }
        public virtual int In_Diario_Minuto_Rodou_Usina { get; set; }
        public virtual string In_Diario_Empilhadeira { get; set; }
        public virtual string In_Diario_Material2 { get; set; }
        public virtual string In_Diario_Origem2 { get; set; }
        public virtual int In_Diario_Taxa_1 { get; set; }
        public virtual int In_Diario_Taxa_2 { get; set; }
        public virtual bool In_Diario_Qualidade_Material_Nok { get; set; }
        public virtual bool In_Diario_Rota_Restrita_Nok { get; set; }
        public virtual string In_Diario_Prefixo_Lote2 { get; set; }
        public virtual int In_Diario_Quantidade_Vagoes2 { get; set; }
        public virtual int In_Diario_Hora_Inicial2 { get; set; }
        public virtual int In_Diario_Minuto_Inicial2 { get; set; }
        public virtual int In_Diario_Hora_Final2 { get; set; }
        public virtual int In_Diario_Minuto_Final2 { get; set; }
        public virtual bool In_Diario_Baliza_Inicial_Lote_Direcao { get; set; }
        public virtual bool In_Diario_Baliza_Final_Lote_Direcao { get; set; }
        public virtual int In_Diario_Restricao_Qualidade_Material { get; set; }
        public virtual int In_Diario_Restricao_Rota_Restrita { get; set; }
        public virtual int In_Diario_Peso_Lote1 { get; set; }
        public virtual int In_Diario_Peso_Lote2 { get; set; }
        public virtual Logbook Logbook { get; set; }
    }
}
