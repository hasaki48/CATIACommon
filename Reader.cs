using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CATIACommon
{
    internal class Reader
    {
        public static IEnumerable<Point> LoadPointData()
        {
            using (TextFieldParser parser = new TextFieldParser("../../points.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    Point point = new Point(double.Parse(fields[0]), double.Parse(fields[1]), double.Parse(fields[2]));
                    yield return point;
                }
            }
        }

        public static IEnumerable<TwoPointLine> LoadTwoPointsData()
        {
            using (TextFieldParser parser = new TextFieldParser("../../lines.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int row = 0;
                Point start_point = new Point(0, 0, 0);
                while(!parser.EndOfData)
                {
                    //string[] A_fields = parser.ReadFields();
                    //Point A = new Point(double.Parse(A_fields[0]), double.Parse(A_fields[1]), double.Parse(A_fields[2]));
                    if(row == 0)
                    {
                        row += 1;
                        string[] A_fields = parser.ReadFields();
                        Point A = new Point(double.Parse(A_fields[0]), double.Parse(A_fields[1]), double.Parse(A_fields[2]));
                        start_point = A;
                    }
                    string[] B_fields = parser.ReadFields();
                    Point B = new Point(double.Parse(B_fields[0]), double.Parse(B_fields[1]), double.Parse(B_fields[2]));
                    TwoPointLine line = new TwoPointLine(start_point, B);
                    start_point = B;
                    yield return line;
                }
            }
        }
    }

}
