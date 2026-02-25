namespace Vale.Tops.Application.Presentation.AssetManager.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    //internal sealed class Configuration : DbMigrationsConfiguration<Vale.Tops.Application.Presentation.AssetManager.Models.ApplicationDbContext>
    //{
    //    public Configuration()
    //    {
    //        AutomaticMigrationsEnabled = false;
    //    }

    //    protected override void Seed(Vale.Tops.Application.Presentation.AssetManager.Models.ApplicationDbContext context)
    //    {
    //        //  This method will be called after migrating to the latest version.

    //        //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
    //        //  to avoid creating duplicate seed data.
    //    }
    //}

    public class Configuration : DbMigrationsConfiguration<Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager.WriteReadContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Vale.Tops.Integration.Infrastructure.DataBase.Repository.Source.PS_MSCS_SQL.AssetManager.WriteReadContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
