using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CATIACommon
{
    class Point
    {
        public double x;
        public double y;
        public double z;

        public Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    class TwoPointLine
    {
        public Point A;
        public Point B;

        public TwoPointLine(Point a, Point b)
        {
            A = a;
            B = b;
        } 
    }
}
