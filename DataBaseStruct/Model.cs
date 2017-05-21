using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBaseStruct
{
   public class Model
    {
        public class Customer
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int CustomerId { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Patronymic { get; set; }
            public virtual List<Project> Projects { get; set; }
        }

        public class Project
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ProjectId { get; set; }
            public int ProjectNumber { get; set; }
            public int ProgectName { get; set; }
            public virtual Customer Customer { get; set; }
            public virtual SchemeOfBuilding Scheme { get; set; }
        }

        public class SchemeOfBuilding
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int SchemeId { get; set; }
            public double Width { get; set; }
            public double Lenght { get; set; }
            public virtual Project ProjectNumber { get; set; }
            public virtual List<InstallationPosition> Positions { get; set; }
        }

        public class InstallationPosition
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int PositionId { get; set; }
            public int Coord_X { get; set; }
            public int Coord_Y { get; set; }
            public virtual SchemeOfBuilding Scheme { get; set; }
            public virtual PlacmentOfModules Placment { get; set; }
        }

        public class PlacmentOfModules
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int PlacementId { get; set; }
            public virtual InstallationPosition Position { get; set; }
            public virtual ModelsOfModules Model { get; set; }
        }

        public class ModelsOfModules
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int ModuleId { get; set; }
            public string ModelName { get; set; }
            public string ModelRadius { get; set; }
        }
    }
}
