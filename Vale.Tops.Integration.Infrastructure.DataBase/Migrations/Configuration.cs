using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Web;

namespace Vale.Tops.Integration.Infrastructure.DataBase.Migrations
{
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
