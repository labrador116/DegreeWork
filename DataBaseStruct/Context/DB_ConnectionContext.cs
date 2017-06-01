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

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<SchemeOfBuilding> Schemes { get; set; }
        public DbSet<InstallationPosition> InstallationPositions { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<CoordinateOfCorner> CornerCoordinates { get; set; }
        public DbSet<ControlPoint> Points { get; set; }
        public DbSet<PlacmentOfModules> Placements { get; set; }
        public DbSet<ModelsOfModules> Models { get; set; }
    }
}
