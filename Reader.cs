using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

// 读取各种数据

namespace CATIACommon
{
    internal class Reader
    {
        public static IEnumerable<My_Point> LoadPointData()
        // 读取三维点的数据
        {
            using (TextFieldParser parser = new TextFieldParser("../../points.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    My_Point point = new My_Point(double.Parse(fields[0]), double.Parse(fields[1]), double.Parse(fields[2]));
                    yield return point;
                }
            }
        }

        public static IEnumerable<My_TwoPointLine> LoadTwoPointsData()
        // 读取由三维点构成的直线数据
        {
            using (TextFieldParser parser = new TextFieldParser("../../lines.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int row = 0;
                My_Point start_point = new My_Point(0, 0, 0);
                while(!parser.EndOfData)
                {
                    if(row == 0)
                    {
                        row += 1;
                        string[] A_fields = parser.ReadFields();
                        My_Point A = new My_Point(double.Parse(A_fields[0]), double.Parse(A_fields[1]), double.Parse(A_fields[2]));
                        start_point = A;
                    }
                    string[] B_fields = parser.ReadFields();
                    My_Point B = new My_Point(double.Parse(B_fields[0]), double.Parse(B_fields[1]), double.Parse(B_fields[2]));
                    My_TwoPointLine line = new My_TwoPointLine(start_point, B);
                    start_point = B;
                    yield return line;
                }
            }
        }

        public static IEnumerable<SingleGrading> LoadGradingData()
        {
            using(TextFieldParser parser = new TextFieldParser("../../Grading.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int row = 0;
                while(!parser.EndOfData)
                {
                    
                    if (row == 0)
                    {
                        row += 1;
                        _ = parser.ReadFields();
                        continue;
                    }
                    string[] fields = parser.ReadFields();
                    SingleGrading grading = new SingleGrading(fields[0], double.Parse(fields[1]), double.Parse(fields[2]));
                    yield return grading;
                }
            }
        }
    }

}
