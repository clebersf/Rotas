using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Integration.Infrastructure.DataBase.Mappings
{
    public class vwCockpit01dbMapping : EntityTypeConfiguration<vwCockpit01DB>
    {
        public vwCockpit01dbMapping()
        {

            this.ToTable("VWCOCKPIT01DB", "B81");
            this.HasKey(e => e.Virador);
            this.HasKey(e => e.CD_PONTO);
            this.HasKey(e => e.Data_Ultima_Coleta);
            this.Property(e => e.Virador).HasColumnName("VIRADOR");
            this.Property(e => e.CD_EQP).HasColumnName("CD_EQP");
            this.Property(e => e.NOM_EQP).HasColumnName("NOM_EQP");
            this.Property(e => e.Local_Instalacao_SAPPM).HasColumnName("LOCAL_INSTALACAO_SAPPM");
            this.Property(e => e.CD_PONTO).HasColumnName("CD_PONTO");
            this.Property(e => e.NOM_PONTO).HasColumnName("NOM_PONTO");
            this.Property(e => e.CD_Alarme).HasColumnName("CD_ALARME");
            this.Property(e => e.DS_Alarme).HasColumnName("DS_ALARME");
            this.Property(e => e.Criticidade).HasColumnName("CRITICIDADE");
            this.Property(e => e.Data_Ultima_Coleta).HasColumnName("DATA_ULTIMA_COLETA");
 
        }
    }
}
