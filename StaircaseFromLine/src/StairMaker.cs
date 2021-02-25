using Elements;
using Elements.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace StaircaseFromLine
{
    public class StairMaker
    {
        public List<Stair> Stairs { get; private set; }
        private static string _texturePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Textures/Concrete_512.jpg");
        public StairMaker(double maximumRiserHeight, double threadDepth, double structuralDepth, double runWidth, List<Line> stairPaths, List<Level> levels)
        {

            Stairs = new List<Stair>();

            for (int i = 0; i < levels.Count - 1; i++)
            {
                double stairHeight = levels[i + 1].Elevation - levels[i].Elevation;

                // Calculate actual stair dimensions
                int riserNumber = Convert.ToInt32(Math.Ceiling(stairHeight / maximumRiserHeight));
                double actualRiserHeigh = stairHeight / riserNumber;

                // Create a flight of step for every line in the path

                double elevation = levels[i].Elevation;
                foreach (Line flightLine in stairPaths)
                {
                    elevation = elevation + StairFlightMaker(flightLine, threadDepth, actualRiserHeigh, runWidth, elevation,structuralDepth);
                }
            }



        }

        private double StairFlightMaker(Line flightLine, double threadDepth, double actualRiserHeigh, double runWidth, double elevation, double structuralDepth)
        {
            // Number of thread in the flight
            int threadNumber = Convert.ToInt32(Math.Ceiling(flightLine.Length() / threadDepth));
            double flightHeight = threadNumber * actualRiserHeigh;
            List<Vector3> profileVertices = new List<Vector3>();

            Vector3 riserVector = Vector3.ZAxis * actualRiserHeigh;
            Vector3 threadVector = Vector3.XAxis * threadDepth;

            // Add first point
            profileVertices.Add(new Vector3());

            for (int i = 0; i < threadNumber; i++)
            {
                profileVertices.Add(profileVertices[i * 2] + riserVector);
                profileVertices.Add(profileVertices[i * 2] + riserVector + threadVector);
            }

            
            profileVertices.Add(CalculateEndStairUnderside(structuralDepth,riserVector,threadVector,profileVertices.Last()));
            profileVertices.Add(CalculateStartStairUnderside(structuralDepth,riserVector,threadVector));

            Polygon profile = new Polygon(profileVertices);

            Vector3 flightStartingPoint = flightLine.Start + Vector3.ZAxis * elevation + Vector3.ZAxis.Cross(flightLine.Direction()).Negate() * runWidth / 2;
            Transform transform = new Transform(flightStartingPoint, flightLine.Direction(), Vector3.ZAxis);

            var extrude1 = new Elements.Geometry.Solids.Extrude(profile, runWidth, Vector3.YAxis, false);
            // var extrude2 = new Elements.Geometry.Solids.Extrude(profile, runWidth / 2, Vector3.YAxis.Negate(), false);
            var geomRep = new Representation(new List<Elements.Geometry.Solids.SolidOperation>() { extrude1 });
            var stairMaterial = new Material("Concrete", Colors.White, 0.5, 0.1, _texturePath);

            Stairs.Add(new Stair(0, actualRiserHeigh, actualRiserHeigh, 0, profile,
            transform, stairMaterial, geomRep, false, Guid.NewGuid(), ""));

            return flightHeight;
        }

        private Vector3 CalculateStartStairUnderside(double structuralDepth, Vector3 riserVector, Vector3 threadVector)
        {
            Line ae = new Line(new Vector3(),Vector3.XAxis,10);

            Vector3 ac = riserVector + threadVector;
            Vector3 cd = ac.Unitized().Negate().Cross(Vector3.YAxis)*structuralDepth;
            Line de = new Line(ac+cd,ac.Negate(),10);

            Vector3 e = new Vector3();
            ae.Intersects(de,out e,true,true);

            return e;
        }
        
        private Vector3 CalculateEndStairUnderside(double structuralDepth, Vector3 riserVector, Vector3 threadVector, Vector3 lastPoint)
        {
            Line cd = new Line(lastPoint,Vector3.ZAxis.Negate(),10);

            Vector3 ac = riserVector + threadVector;
            Vector3 ae = ac.Unitized().Cross(Vector3.YAxis.Negate())*structuralDepth;
            Line ed = new Line(lastPoint + ac.Negate() + ae,ac.Negate(),10);

            Vector3 e = new Vector3();
            cd.Intersects(ed,out e,true,true);

            return e;
        }
    }
}