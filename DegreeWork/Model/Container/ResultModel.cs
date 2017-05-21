using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmCourseWork.Container
{
   public class ResultModel 
    {
        private double _ratio;
        private Chromosome _chromosome;

        public double Ratio { get => _ratio; set => _ratio = value; }
        public Chromosome Chromosome { get => _chromosome; set => _chromosome = value; }
    
        
    }
}
