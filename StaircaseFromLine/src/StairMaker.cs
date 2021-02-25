using Elements;
using Elements.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StaircaseFromLine
{
    public class StairMaker
    {
        public List<Stair> Stairs { get; private set; }

        public StairMaker(double maximumRiserHeight, double threadDepth, double runWidth, List<Line> stairPaths, List<Level> levels)
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

                    elevation = elevation + StairFlightMaker(flightLine, threadDepth, actualRiserHeigh, runWidth, elevation);
                }
            }



        }

        private double StairFlightMaker(Line flightLine, double threadDepth, double actualRiserHeigh, double runWidth, double elevation)
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

            profileVertices.Add(profileVertices.Last() + Vector3.ZAxis * (-0.5));
            profileVertices.Add(profileVertices[0] + Vector3.XAxis * (0.5));

            Polygon profile = new Polygon(profileVertices);

            Vector3 flightStartingPoint = flightLine.Start + Vector3.ZAxis * elevation + Vector3.ZAxis.Cross(flightLine.Direction()).Negate() * runWidth / 2;
            Transform transform = new Transform(flightStartingPoint, flightLine.Direction(), Vector3.ZAxis);

            var extrude1 = new Elements.Geometry.Solids.Extrude(profile, runWidth, Vector3.YAxis, false);
            // var extrude2 = new Elements.Geometry.Solids.Extrude(profile, runWidth / 2, Vector3.YAxis.Negate(), false);
            var geomRep = new Representation(new List<Elements.Geometry.Solids.SolidOperation>() { extrude1 });
            var coreMatl = BuiltInMaterials.Concrete;

            Stairs.Add(new Stair(0, actualRiserHeigh, actualRiserHeigh, 0, profile,
            transform, coreMatl, geomRep, false, Guid.NewGuid(), ""));

            return flightHeight;
        }
    }
}