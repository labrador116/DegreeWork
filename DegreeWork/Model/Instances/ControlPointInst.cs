using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegreeWork.Model.Instances
{
   public class ControlPointInst
    {
        int X;
        int Y;

        public ControlPointInst (int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X1 { get => X; set => X = value; }
        public int Y1 { get => Y; set => Y = value; }
    }
}
