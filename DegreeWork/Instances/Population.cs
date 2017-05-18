using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmCourseWork.Container
{
    class Population
    {
        private List<Chromosome> _populationContainer;

        public Population()
        {
            _populationContainer = new List<Chromosome>();
        }

        internal List<Chromosome> GetSetPopulationContainer { get => _populationContainer; set => _populationContainer = value; }
    }
}
