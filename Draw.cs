using HybridShapeTypeLib;
using INFITF;
using MECMOD;

namespace CATIACommon
{
    internal class Draw
    {
        internal static void DrawPoints(Application CATIA)
        {
            foreach (Point point in Reader.LoadPointData())
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
            foreach(TwoPointLine line in Reader.LoadTwoPointsData())
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
    }


}