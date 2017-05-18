using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticAlgorithmCourseWork.ChromosomeModel;
using GeneticAlgorithmCourseWork.SpaceParam;

namespace GeneticAlgorithmCourseWork.Container
{
  public  class Chromosome
    {
        private List<ChromosomeModel.Gene> _container;
        private int areaWidth;
        private int areaHeight;
        
        public Chromosome()
        {
            _container = new List<Gene>();
            areaWidth = SingleSpaceParams.getInstance().Width;
            areaHeight = SingleSpaceParams.getInstance().Height;
        }

        public List<Gene> Container { get => _container; set => _container = value; }
        public int AreaWidth { get => areaWidth; set => areaWidth = value; }
        public int AreaHeight { get => areaHeight; set => areaHeight = value; }
    }
}
