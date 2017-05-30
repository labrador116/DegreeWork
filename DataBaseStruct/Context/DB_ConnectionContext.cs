using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using static DataBaseStruct.Model;

namespace DataBaseStruct
{
   public class DB_ConnectionContext : DbContext
    {
        public DB_ConnectionContext() : base("DegreeProjDB")
        {

        }

        public DB_ConnectionContext(string dbConnection) : base(dbConnection)
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<SchemeOfBuilding>()
             //   .HasRequired(p => p.ProjectNumber)
               // .WithRequiredDependent(c => c.Scheme);
            /*modelBuilder.Entity<SchemeOfBuilding>()
                .HasOptional(p => p.ProjectNumber)
                .WithOptionalDependent()
                .Map(p=>p.MapKey("SchemeKey"));

            modelBuilder.Entity<Project>()
                .HasOptional(c => c.Scheme)
                .WithOptionalPrincipal()
                .Map(c => c.MapKey("ProjectKey"));*/

           modelBuilder.Entity<PlacmentOfModules>()
                .HasRequired(pv => pv.Position)
                .WithOptional(cv => cv.Placment);

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<SchemeOfBuilding> Schemes { get; set; }
        public DbSet<InstallationPosition> Positions { get; set; }
        public DbSet<PlacmentOfModules> Placements { get; set; }
        public DbSet<ModelsOfModules> Models { get; set; }


    }
}
