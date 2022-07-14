// 定义各种数据类型

using System.Collections.Generic;

namespace CATIACommon
{
    class My_Point_2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public My_Point_2D(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    class My_Point
    // 三维点
    {
        public double x;
        public double y;
        public double z;

        public My_Point(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    class My_TwoPointLine
    // 由两个三维点定义的直线
    {
        public My_Point A;
        public My_Point B;

        public My_TwoPointLine(My_Point a, My_Point b)
        {
            A = a;
            B = b;
        } 
    }

    class SingleGrading
    {
        public string type;
        public double length_or_height;
        public double incline;

        public SingleGrading(string type, double length_or_height,double incline)
        {
            this.type = type;
            this.length_or_height = length_or_height;
            this.incline = incline;
        }
    }
}
