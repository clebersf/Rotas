
namespace Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Vale.Tops.Domain;
    using Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager.Mappings;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public partial class WriteReadContext : DbContext
    {
        public WriteReadContext()
            : base("name=Connection_AssetManager")
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
        }
        public virtual DbSet<Graph>  Graph { get; set; }
        public virtual DbSet<rTagConveyorClean> rTagConveyorClean { get; set; }
        public virtual DbSet<Destination> Destination { get; set; }
        public virtual DbSet<Origin> Origin { get; set; }
        public virtual DbSet<OxD> OxD { get; set; }
        public virtual DbSet<Line> Line { get; set; }
        public virtual DbSet<rProductionRoute> rProductionRoute { get; set; }
        public virtual DbSet<rProductionBoarding> rProductionBoarding { get; set; }
        public virtual DbSet<rProductionStock> rProductionStock { get; set; }
        public virtual DbSet<rProductionOrigin> rProductionOrigin { get; set; }
        public virtual DbSet<Production> Production { get; set; }
        public virtual DbSet<Rule> Rule { get; set; }
        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Plc> Plc { get; set; }
        public virtual DbSet<rTagInformation> rTagInformation { get; set; }
        public virtual DbSet<rInstrumentMeasure> rInstrumentMeasure { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<rTagGroup> rTagGroup { get; set; }
     

        public virtual DbSet<Information> Information{ get; set; }

        public virtual DbSet<rLogbookEPEE> rLogbookEPEE { get; set; }
        public virtual DbSet<Domain.Type> Type { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<Resource> Resource { get; set; }
        public virtual DbSet<rLocationHistory> rLocationHistory { get; set; }
        public virtual DbSet<rLocationEnable> rLocationEnable { get; set; }
        public virtual DbSet<CheckList> CheckList { get; set; }
        public virtual DbSet<History> History { get; set; }
        public virtual DbSet<rRuleNavbar> rRuleNavbar { get; set; }
        public virtual DbSet<Berth> Berth { get; set; }
        public virtual DbSet<Logbook> Logbook { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<Instrument> Instrument { get; set; }
        public virtual DbSet<Boarding> Boarding { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<rRouteOxD> rRouteOxD { get; set; }
        public virtual DbSet<rRouteActive> rRouteActive { get; set; }
        public virtual DbSet<rRouteReplace> rRouteReplace { get; set; }
        public virtual DbSet<rRouteTravel> rRouteTravel{ get; set; }
        public virtual DbSet<rRouteGraphLocation> rRouteGraphLocation { get; set; }
        public virtual DbSet<rRouteQueue> rRouteQueue { get; set; }
        public virtual DbSet<RouteGraph> RouteGraph { get; set; }
        public virtual DbSet<rRouteSequence> rRouteSequence { get; set; }
        public virtual DbSet<rRouteGraphSequence> rRouteSequenceGraph { get; set; }
        public virtual DbSet<Compartment> Compartment { get; set; }
        public virtual DbSet<Active> Active { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Flow> Flow { get; set; }
        public virtual DbSet<Step> Step { get; set; }
        public virtual DbSet<Load> Load { get; set; }
        public virtual DbSet<Reference> Reference { get; set; }
        public virtual DbSet<Consistency> Consistency { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Command> Command { get; set; }
        public virtual DbSet<Damper> Dumper { get; set; }
        public virtual DbSet<Reversal> Reversal { get; set; }
        public virtual DbSet<Tripper> Tripper { get; set; }
        public virtual DbSet<Feeder> Feeder { get; set; }
        public virtual DbSet<Conveyor> Conveyor { get; set; }
        public virtual DbSet<rConveyorCapacity> rConveyorCapacity { get; set; }
        public virtual DbSet<rConveyorConsumption> rConveyorConsumption { get; set; }
        public virtual DbSet<rConveyorLenght> rConveyorLenght { get; set; }
        public virtual DbSet<rConveyorSpeed> rConveyorSpeed { get; set; }
        public virtual DbSet<rConveyorScale> rConveyorScale { get; set; }
        public virtual DbSet<rConveyorOutputPosition> rConveyorOutputPosition { get; set; }
        public virtual DbSet<rConveyorInputPosition> rConveyorInputPosition { get; set; }

        public virtual DbSet<rRouteGraphGpv> RouteGraphGpv { get; set; }

        public virtual DbSet<rTagWrite> rTagWrite { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ///////////////////////// LOCATION ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:1 com Location


            modelBuilder.Entity<Conveyor>() // Correia tranportador щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Plc>() // Plc щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<rLocationValue>() // rLocationValue щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<rLocationEnable>() // rLocationEnable щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Compartment>() // Compartment щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<rRouteReplace>() // rRouteReplace щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Origin>() // Origin щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Reversal>() // Reversal щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Tripper>() // Tripper щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Feeder>() // Feeder щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Damper>() // Damper щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Command>() // Command щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Permission>() // Permission щ um Location
               .HasRequired(e => e.Location);

            modelBuilder.Entity<Consistency>() // Consistency щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Reference>() // Reference щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<OxD>() // OxD щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Line>() // Line щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Route>() // Route щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Active>() // Active щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Step>() // Step щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Position>() // Position щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Flow>() // Flow щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Load>() // Load щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Instrument>() // Instrument щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Destination>() // Destination щ um Location
                .HasRequired(e => e.Location);

            modelBuilder.Entity<Berth>() // Berth щ um Location
               .HasRequired(e => e.Location);

            modelBuilder.Entity<Stock>() // Stock щ um Location
               .HasRequired(e => e.Location);

            modelBuilder.Entity<Boarding>() // Boarding щ um Location
               .HasRequired(e => e.Location);

            modelBuilder.Entity<rInstrumentMeasure>() // rInstrumentMeasure щ um Location
               .HasRequired(e => e.Location);

            modelBuilder.Entity<Information>() // Information щ um Location
               .HasRequired(e => e.Location);

            // Configuraчуo de atributos de Location
            modelBuilder.Entity<Location>()
               .Property(e => e.Name)
               .IsUnicode(false);


            ////////////////////////////// Relacionamentos 1:N Location


            modelBuilder.Entity<Location>()
               .HasMany(e => e.rRouteSequence)
               .WithRequired(e => e.Route)
               .HasForeignKey(e => e.RouteId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
               .HasMany(e => e.rRouteSequence)
               .WithRequired(e => e.Location)
               .HasForeignKey(e => e.LocationId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationA)
                .WithRequired(e => e.LocationA)
                .HasForeignKey(e => e.LocationAId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationB)
                .WithRequired(e => e.LocationB)
                .HasForeignKey(e => e.LocationBId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
               .HasMany(e => e.Parent)
               .WithOptional(e => e._Location)
               .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Location>()
               .HasMany(e => e.rLocationDataHistory)
               .WithOptional(e => e.Location)
               .HasForeignKey(e => e.LocationId);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.rProductionBoarding)
                .WithRequired(e => e.Berth)
                .HasForeignKey(e => e.BerthId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
               .HasMany(e => e.rConveyorInputPosition)
               .WithOptional(e => e.LocationA)
               .HasForeignKey(e => e.LocationAId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
               .HasMany(e => e.rConveyorInputPosition)
               .WithOptional(e => e.LocationB)
               .HasForeignKey(e => e.LocationBId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
               .HasMany(e => e.rConveyorOutputPosition)
               .WithOptional(e => e.Location)
               .HasForeignKey(e => e.LocationId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
               .HasMany(e => e.rConveyorScale)
               .WithOptional(e => e.Location)
               .HasForeignKey(e => e.LocationId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
               .HasMany(e => e.rProductionRoute)
               .WithRequired(e => e.Route)
               .HasForeignKey(e => e.RouteId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.rLocationHistory)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Logs)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);


            ///////////////////////// Type ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:N com Type

            modelBuilder.Entity<Domain.Type>()
                .HasMany(e => e.Location)
                .WithRequired(e => e.Type)
                .WillCascadeOnDelete(false);


            ///////////////////////// Production ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:1 com Prodution
            ///

            modelBuilder.Entity<rProductionStock>() // Flow щ um Location
                .HasRequired(e => e.Production);
            modelBuilder.Entity<rProductionBoarding>() // Flow щ um Location
                .HasRequired(e => e.Production);


            ////////////////////////////// Relacionamentos 1:N com Prodution
            modelBuilder.Entity<Production>()
                .HasMany(e => e.rProductionOrigin)
                .WithRequired(e => e.Production)
                .HasForeignKey(e => e.ProductionId)
                .WillCascadeOnDelete(true);


            ///////////////////////// Instrument ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:N com instrument
            ///

            modelBuilder.Entity<Instrument>()
                .HasMany(e => e.rProductionRoutes)
                .WithRequired(e => e.Instrument)
                .HasForeignKey(e => e.InstrumentId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Instrument>()
                .HasMany(e => e.rProductionStocks)
                .WithRequired(e => e.Instrument)
                .HasForeignKey(e => e.InstrumentId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Instrument>()
                .HasMany(e => e.rProductionBoardings)
                .WithRequired(e => e.Instrument)
                .HasForeignKey(e => e.InstrumentId)
                .WillCascadeOnDelete(true);


            ///////////////////////// Rule ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:1 com Rule
            ///

            modelBuilder.Entity<rRuleNavbar>() // Information щ um Location
               .HasRequired(e => e.Rule);



            ///////////////////////// Application ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:N com Application
            ///
 
            modelBuilder.Entity<Application>()
                .HasMany(e => e.Logs)
                .WithRequired(e => e.Application)
                .HasForeignKey(e => e.ApplicationId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Application>()
                .HasMany(e => e.Rules)
                .WithRequired(e => e.Application)
                .HasForeignKey(e => e.ApplicationId)
                .WillCascadeOnDelete(true);

            ///////////////////////// Route ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:1 com Route
            ///

            modelBuilder.Entity<rRouteActive>() // Information щ um Location
               .HasRequired(e => e.Location);

            modelBuilder.Entity<rRouteQueue>() // Information щ um Location
               .HasRequired(e => e.Location);


            modelBuilder.Entity<rRouteTravel>() // Information щ um Location
               .HasRequired(e => e.Location);

            modelBuilder.Entity<rRouteReplace>() // Information щ um Location
               .HasRequired(e => e.Location);


            ///////////////////////// Tag ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:1 com Tag
            ///

            modelBuilder.Entity<rTagWrite>() // Information щ um Location
               .HasRequired(e => e.Tag);

            modelBuilder.Entity<rTagInformation>() // Information щ um Location
               .HasRequired(e => e.Tag);

            modelBuilder.Entity<rTagConveyorClean>() // Information щ um Location
               .HasRequired(e => e.Tag);


            ////////////////////////////// Relacionamentos 1:N com Tag
            ///

            modelBuilder.Entity<Tag>()
               .HasMany(e => e.rTagGroup)
               .WithOptional(e => e.Tag)
               .HasForeignKey(e => e.TagId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tag>()
               .HasMany(e => e.rInstrumentMeasure)
               .WithRequired(e => e.Tag)
               .HasForeignKey(e => e.TagId)
               .WillCascadeOnDelete(false);


            ///////////////////////// TagGroup ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:N com TagGroup
            ///

            modelBuilder.Entity<rTagGroup>()
               .HasMany(e => e.Parent)
               .WithOptional(e => e._rTagGroup)
               .HasForeignKey(e => e.ParentId);



            ///////////////////////// Plc ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:N com Plc
            ///

            modelBuilder.Entity<Plc>()
               .HasMany(e => e.Tag)
               .WithRequired(e => e.Plc)
               .HasForeignKey(e => e.PlcId)
               .WillCascadeOnDelete(false);


            ///////////////////////// OxD ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:N com OxD
            ///

            //Relacionamentos entidade rRouteOxD
            modelBuilder.Entity<OxD>()
                .HasMany(e => e.rRouteOxD)
                .WithRequired(e => e.OxD)
                .HasForeignKey(e => e.OxDId)
                .WillCascadeOnDelete(false);

            //Relacionamentos entidade rRouteOxD
            modelBuilder.Entity<Location>()
                .HasMany(e => e.rRouteOxD)
                .WithRequired(e => e.Location)
                .HasForeignKey(e => e.LocationId)
                .WillCascadeOnDelete(false);


            ///////////////////////// Conveyor ///////////////////
            ///
            ////////////////////////////// Relacionamentos 1:1 com Conveyor
            ///

            modelBuilder.Entity<rConveyorConsumption>() // Information щ um Location
               .HasRequired(e => e.Conveyor);

            modelBuilder.Entity<rConveyorCapacity>() // Information щ um Location
               .HasRequired(e => e.Conveyor);

            modelBuilder.Entity<rConveyorLenght>() // Information щ um Location
               .HasRequired(e => e.Conveyor);

            modelBuilder.Entity<rConveyorSpeed>() // Information щ um Location
               .HasRequired(e => e.Conveyor);

            modelBuilder.Entity<rTagConveyorClean>() // Information щ um Location
               .HasRequired(e => e.Conveyor);


            ///////////////////////// Logbook ///////////////////
            ///
            /// 
            ////////////////////////////// Relacionamentos 1:1 com Logbook
            ///
            /// 

            modelBuilder.Entity<rLogbookEPEE>() // Information щ um Location
               .HasRequired(e => e.Logbook);

            modelBuilder.Entity<rLogbookVV>() // Information щ um Location
               .HasRequired(e => e.Logbook);

            modelBuilder.Entity<rLogbookHistory>() // Information щ um Location
               .HasRequired(e => e.Logbook);


            ///////////////////////// RouteGraph ///////////////////
            ///
            /// 
            ////////////////////////////// Relacionamentos 1:1 com RouteGraph
            ///
            /// 
            modelBuilder.Entity<RouteGraph>()
               .HasMany(e => e.rRouteGraphSequence)
               .WithRequired(e => e.RouteGraph)
               .HasForeignKey(e => e.RouteGraphId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<rRouteGraphGpv>()
                .HasRequired(e => e.RouteGraph);

            modelBuilder.Entity<rRouteGraphLocation>()
                .HasRequired(e => e.RouteGraph);

        }
    }

    public class ReadOnlyContext : DbContext
    {
        public ReadOnlyContext()
            : base("Connection_AssetManager")
        {
            Database.SetInitializer<ReadOnlyContext>(null);
        }

        
        public DbSet<vw_Plc_Read_Time> vw_Plc_Read_Time_ReadOnly { get; set; }
        public DbSet<vwConveyorInformation> vwConveyorInformation { get; set; }
        public DbSet<vwRouteActiveList> vwRouteActiveList { get; set; }
        public DbSet<vwRouteLog> vwRouteLogReadOnly { get; set; }
        public DbSet<vw_Route_Consistency_Feeder> vw_Route_Consistency_Feeder { get; set; }
        public DbSet<vw_Route_Consistency_Damper> vw_Route_Consistency_Damper { get; set; }
        public DbSet<vw_Route_Consistency_Reversal> vw_Route_Consistency_Reversal { get; set; }
        public DbSet<vw_Route_Consistency_Tripper> vw_Route_Consistency_Tripper { get; set; }
        public DbSet<vw_Route_Consistency_Limit> vw_Route_Consistency_Limit { get; set; }
        public DbSet<vw_Route_Consistency_Rule> vw_Route_Consistency_Rule { get; set; }
        public DbSet<vw_Route_Tag_Eqp_Permission> vw_Route_Tag_Eqp_Permission { get; set; }
        public DbSet<vw_Route_Tag_Eqp_O_Permission> vw_Route_Tag_Eqp_O_Permission { get; set; }
        public DbSet<vw_Route_Tag_Eqp_D_Permission> vw_Route_Tag_Eqp_D_Permission { get; set; }
        public DbSet<vw_Route_Tag_Feeder_Command> vw_Route_Tag_Feeder_Command { get; set; }
        public DbSet<vw_Route_Tag_Feeder_Permission> vw_Route_Tag_Feeder_Permission { get; set; }
        public DbSet<vw_Route_Tag_Damper_Command> vw_Route_Tag_Damper_Command { get; set; }
        public DbSet<vw_Route_Tag_Damper_Permission> vw_Route_Tag_Damper_Permission { get; set; }
        public DbSet<vw_Route_Tag_OxD_Permission> vw_Route_Tag_OxD_Permission { get; set; }
        public DbSet<vw_Route_Tag_Reversal_Command> vw_Route_Tag_Reversal_Command { get; set; }
        public DbSet<vw_Route_Tag_Reversal_Permission> vw_Route_Tag_Reversal_Permission { get; set; }
        public DbSet<vw_Route_Tag_Tripper_Command> vw_Route_Tag_Tripper_Command { get; set; }
        public DbSet<vw_Route_Tag_Tripper_Permission> vw_Route_Tag_Tripper_Permission { get; set; }
        public DbSet<vw_Agent_History_Performance> vw_Agent_History_Performance_ReadOnly { get; set; }
        public DbSet<vw_Route_Consistency> vw_Route_Consistency_ReadOnly { get; set; }

        // Adicione isto no seu DataContext.cs, dentro de OnModelCreating
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
        }

        public virtual List<fn_RouteReplacer_I_Route_O_Route> fn_RouteReplacer_I_Route_O_Route(long RouteId)
        {
            List<fn_RouteReplacer_I_Route_O_Route> data = new List<fn_RouteReplacer_I_Route_O_Route>();
            try
            {
                data =
                Database.SqlQuery<fn_RouteReplacer_I_Route_O_Route>(
                "select * from dbo.fn_RouteReplacer_I_Route_O_Route(@RouteId)",
                new SqlParameter("@RouteId", RouteId))
            .ToList();
            }
            catch(Exception)
            {
               // sem tratamento
            }
            return data;
        }

        public void sp_Tag_Consistency_Read()
        {
            try
            {                
                Database.ExecuteSqlCommand("EXEC sp_Tag_Consistency_Read");
            }
            catch
            { 
                //sem tratamento 
            }
        }
        public void sp_Route_Tag_Bool_Activate()
        {
            try
            {
                Database.ExecuteSqlCommand("EXEC sp_Route_Tag_Bool_Activate");
            }
            catch
            { 
                //sem tratamento
            }
        }

        public void sp_Route_Tag_Feeder_Activate()
        {
            try
            {
                Database.ExecuteSqlCommand("EXEC sp_Route_Tag_Feeder_Activate");
            }
            catch
            { 
                //sem tratamento
            }
        }

        public void sp_Tag_Deactivate()
        {
            try
            {
                Database.ExecuteSqlCommand("EXEC sp_Tag_Deactivate");
            }
            catch
            {
                //sem tratamento
            }
        }

        public void sp_Route_Tag_Write_Information()
        {
            try
            {
                Database.ExecuteSqlCommand("EXEC sp_Route_Tag_Write_Information");
            }
            catch
            { 
                //sem tratamento
            }
        }


        
    }

    public class VS50ReadOnlyContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<vwRateioGpvPublish>().ToTable("vwRateioGpvPublish");
            modelBuilder.Entity<vwRateioGpvDescPublish>().ToTable("vwRateioGpvDescPublish");
            modelBuilder.Entity<vwRateioGpvAllPublish>().ToTable("vwRateioGpvAllPublish");
            modelBuilder.Entity<vwRateioGpvAllPublish>().ToTable("vwRateioGpvAllPublish");
        }
        public VS50ReadOnlyContext()
            : base("Connection_ShipOnline")
        {
            Database.SetInitializer<VS50ReadOnlyContext>(null);
        }

        public DbSet<vwRateioGpvPublish> vwRateioGpvPublish { get; set; }

        public DbSet<vwRateioGpvDescPublish> vwRateioGpvDescPublish { get; set; }

        public DbSet<vwRateioGpvAllPublish> vwRateioGpvAllPublish { get; set; }

        
    }
}
