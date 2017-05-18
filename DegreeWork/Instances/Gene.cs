using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticAlgorithmCourseWork.ChromosomeModel
{
   public class Gene : IEquatable<Gene>
    {
        int _radius;
        int _oX;
        int _oY;
        int _numOfPosition;
        String _encodeValue;

        public Gene(int radius, int oX, int oY, int numOfPosition)
        {
            _radius = radius;
            _oX = oX;
            _oY = oY;
            _numOfPosition = numOfPosition;
        }

        public Gene(int radius)
        {
            Radius = radius;
        }

        public int Radius { get => _radius; set => _radius = value; }
        public int OX { get => _oX; set => _oX = value; }
        public int OY { get => _oY; set => _oY = value; }
        public int NumOfPosition { get => _numOfPosition; set => _numOfPosition = value; }
        public string EncodeValue { get => _encodeValue; set => _encodeValue = value; }

        public bool Equals(Gene other)
        {
            if (other == null)
            {
                return false;
            }
            return (this.EncodeValue.Equals(other.EncodeValue));
        }
    }
}
