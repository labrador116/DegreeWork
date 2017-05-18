
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeneticAlgorithmCourseWork.Container
{
   public class Coordinate
    {
        int _coordX;
        int _coordY;
        int _position;

        public int CoordX { get => _coordX; set => _coordX = value; }
        public int CoordY { get => _coordY; set => _coordY = value; }
        public int Position { get => _position; set => _position = value; }
    }
}