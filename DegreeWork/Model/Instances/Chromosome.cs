using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DegreeWork.ChromosomeModel;
using DegreeWork.SpaceParam;

namespace DegreeWork.Container
{
  public  class Chromosome
    {
        private List<ChromosomeModel.Gene> _container;

        public Chromosome()
        {
            _container = new List<Gene>();
        }

        public List<Gene> Container { get => _container; set => _container = value; }
    }
}
