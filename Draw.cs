using HybridShapeTypeLib;
using INFITF;
using MECMOD;
using System;
using System.Collections.Generic;

namespace CATIACommon
{
    internal class Draw
    {
        internal static void DrawPoints(Application CATIA)
        // 画三维点
        {
            foreach (My_Point point in Reader.LoadPointData())
            {
                Part part = (CATIA.ActiveDocument as PartDocument).Part;
                Body body = (part.Bodies.GetItem("零件几何体") as Body);
                HybridShapeFactory hybrid_shape_factory = part.HybridShapeFactory as HybridShapeFactory;
                HybridShapePointCoord coord = hybrid_shape_factory.AddNewPointCoord(point.x, point.y, point.z);
                body.InsertHybridShape(coord);
                part.Update();
            }
        }

        internal static void DrawLines(Application CATIA)
        {
            // 画由两个三维点构成的直线
            foreach (My_TwoPointLine line in Reader.LoadTwoPointsData())
            {
                Part part = (CATIA.ActiveDocument as PartDocument).Part;
                Body body = (part.Bodies.GetItem("零件几何体") as Body);
                HybridShapeFactory hybrid_shape_factory = part.HybridShapeFactory as HybridShapeFactory;

                HybridShapePointCoord A_coord = hybrid_shape_factory.AddNewPointCoord(line.A.x, line.A.y, line.A.z);
                Reference A_ref = part.CreateReferenceFromObject(A_coord);

                HybridShapePointCoord B_coord = hybrid_shape_factory.AddNewPointCoord(line.B.x, line.B.y, line.B.z);
                Reference B_ref = part.CreateReferenceFromObject(B_coord);

                HybridShapeLinePtPt linePtPt = hybrid_shape_factory.AddNewLinePtPt(A_ref, B_ref);
                body.InsertHybridShape(linePtPt);
                part.Update();
            }
        }

        internal static void DrawGrading(Application CATIA)
        {
            Part part = (CATIA.ActiveDocument as PartDocument).Part;
            Body body = (part.Bodies.GetItem("零件几何体") as Body);
            Reference xy_plane_ref = (Reference)part.OriginElements.PlaneXY;
            Sketch sketch = body.Sketches.Add(xy_plane_ref);
            part.InWorkObject = sketch;
            Factory2D factory2D = sketch.OpenEdition();
            My_Point_2D start_point = new My_Point_2D(0, 0);
            Point2D start_point_2D = factory2D.CreatePoint(start_point.X, start_point.Y);
            Reference start_point_ref = part.CreateReferenceFromObject(start_point_2D);

            // 水平轴
            Line2D horizontal_line = sketch.GeometricElements.Item("绝对轴").GetItem("横向") as Line2D;
            Reference horizontal_ref = part.CreateReferenceFromObject(horizontal_line);

            // 起点固定约束
            Constraint point_fixed = sketch.Constraints.AddMonoEltCst(CatConstraintType.catCstTypeReference, start_point_ref);
            point_fixed.Mode = CatConstraintMode.catCstModeDrivingDimension;

            // 固定基准线
            Line2D previous_line = factory2D.CreateLine(0,0,-1,0);
            Reference previous_line_ref = part.CreateReferenceFromObject(previous_line);
            Constraint begin_line_fixed = sketch.Constraints.AddMonoEltCst(CatConstraintType.catCstTypeReference, previous_line_ref);

            foreach(SingleGrading grading in Reader.LoadGradingData())
            {
                if(grading.type == "H")
                {
                    My_Point_2D end_point = new My_Point_2D(start_point.X + grading.length_or_height, start_point.Y);
                    Point2D end_point_2D = factory2D.CreatePoint(end_point.X, end_point.Y);
                    Reference end_point_ref = part.CreateReferenceFromObject(end_point_2D);
                    Line2D line = factory2D.CreateLine(start_point.X, start_point.Y, end_point.X, end_point.Y);
                    line.StartPoint = start_point_2D;
                    line.EndPoint = end_point_2D;
                    Reference line_ref = part.CreateReferenceFromObject(line);

                    // 长度约束
                    Constraint line_length = sketch.Constraints.AddMonoEltCst(CatConstraintType.catCstTypeLength,line_ref);
                    line_length.Mode = CatConstraintMode.catCstModeDrivingDimension;
                    line_length.Dimension.Value = grading.length_or_height;

                    // 水平约束
                    Constraint line_horiztional = sketch.Constraints.AddMonoEltCst(CatConstraintType.catCstTypeHorizontality,line_ref);
                    line_horiztional.Mode = CatConstraintMode.catCstModeDrivingDimension;

                    start_point_ref = end_point_ref;
                    previous_line = line;
                    start_point = end_point;
                    start_point_2D = end_point_2D;
                }
                if(grading.type == "S")
                {
                    double end_point_x = start_point.X + grading.length_or_height * grading.incline;
                    double end_point_y = start_point.Y + grading.length_or_height;
                    My_Point_2D end_point = new My_Point_2D(end_point_x, end_point_y);
                    Point2D end_point_2D = factory2D.CreatePoint(end_point.X, end_point.Y);
                    Reference end_point_ref = part.CreateReferenceFromObject(end_point_2D);
                    Line2D line = factory2D.CreateLine(start_point.X, start_point.Y, end_point_x, end_point_y);
                    line.StartPoint = start_point_2D;
                    line.EndPoint = end_point_2D;
                    previous_line_ref = part.CreateReferenceFromObject(previous_line);
                    Reference line_ref = part.CreateReferenceFromObject(line);

                    // 高度约束
                    Constraint height_constraint = sketch.Constraints.AddBiEltCst(CatConstraintType.catCstTypeDistance,end_point_ref,previous_line_ref);

                    // 放坡坡度约束
                    Constraint angle_constraint = sketch.Constraints.AddBiEltCst(CatConstraintType.catCstTypeAngle, horizontal_ref, line_ref);

                    start_point_ref = end_point_ref;
                    previous_line = line;
                    start_point = end_point;
                    start_point_2D = end_point_2D;
                }
            }


            sketch.CloseEdition();
            part.InWorkObject = sketch;
            part.Update();
        }
    }


}