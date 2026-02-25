using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Vale.Tops.Domain;

namespace Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager.Mappings
{
    public class vw_Presentation_Production_BoardingMapping : EntityTypeConfiguration<vw_Presentation_Production_Boarding>
    {
        public vw_Presentation_Production_BoardingMapping()
        {
            this.ToTable("vw_Presentation_Production_Boarding");
            this.Property(e => e.Id).HasColumnName("Id");
            this.Property(e => e.dhi).HasColumnName("dhi");
            this.Property(e => e.dhf).HasColumnName("dhf");
            this.Property(e => e.Berth).HasColumnName("Berth");
            this.Property(e => e.Compartment).HasColumnName("Compartment");
            this.Property(e => e.Destination).HasColumnName("Destination");
            this.Property(e => e.Load).HasColumnName("Load");
            this.Property(e => e.VVG1).HasColumnName("VVG1");
            this.Property(e => e.VVG2).HasColumnName("VVG2");
            this.Property(e => e.VVG3).HasColumnName("VVG3");
            this.Property(e => e.ERG1).HasColumnName("ERG1");
            this.Property(e => e.ERG2).HasColumnName("ERG2");
            this.Property(e => e.ERG3).HasColumnName("ERG3");
            this.Property(e => e.Final).HasColumnName("Final");
        }
    }
}
