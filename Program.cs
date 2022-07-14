using HybridShapeTypeLib;
using INFITF;
using MECMOD;
using PARTITF;
using ProductStructureTypeLib;
using SPATypeLib;
using NavigatorTypeLib;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using DRAFTINGITF;

namespace CATIACommon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Application CATIA = (INFITF.Application)Marshal.GetActiveObject("Catia.Application");

            string task = "DrawGrading";

            switch (task)
            {
                case "DrawPoints":
                    Draw.DrawPoints(CATIA);
                    break;
                case "DrawLines":
                    Draw.DrawLines(CATIA);
                    break;
                case "DrawGrading":
                    Draw.DrawGrading(CATIA);
                    break;
            }
        }
    }
}
