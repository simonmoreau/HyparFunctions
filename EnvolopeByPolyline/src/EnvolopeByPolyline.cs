using System.Collections.Generic;
using System.Linq;
using System;
using Elements;
using Elements.Geometry;


namespace EnvolopeByPolyline
{
    public static class EnvolopeByPolyline
    {
        /// <summary>
        /// Generate a volume of a given width 
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A EnvolopeByPolylineOutputs instance containing computed results and the model with any new elements.</returns>
        public static EnvolopeByPolylineOutputs Execute(Dictionary<string, Model> inputModels, EnvolopeByPolylineInputs input)
        {
            Polyline polyline = input.BuildingAxis;

            if (polyline == null)
            {
                throw new ArgumentException("Please draw the axe of the building.");
            }

            Polygon perimeter = input.BuildingAxis.Offset(input.BuildingWidth / 2, EndType.Butt).First();

            var envMatl = new Material("envelope", new Color(0.3, 0.7, 0.7, 0.6), 0.0f, 0.0f);
            var envelopes = new List<Envelope>();

            // Create the Envelope at the location's zero plane.
            var output = new EnvolopeByPolylineOutputs(perimeter.ToPolyline().Length() * input.BuildingHeight, perimeter.Area());

            var extrude = new Elements.Geometry.Solids.Extrude(perimeter, input.BuildingHeight, Vector3.ZAxis, false);
            var geomRep = new Representation(new List<Elements.Geometry.Solids.SolidOperation>() { extrude });
            envelopes.Add(new Envelope(perimeter, 0.0, input.BuildingHeight, Vector3.ZAxis, 0.0,
                          new Transform(), envMatl, geomRep, false, Guid.NewGuid(), ""));
            output.Model.AddElements(envelopes);

            var sketch = new ModelCurve(polyline, name: "Centerline Sketch");
            output.Model.AddElement(sketch);
            return output;
        }
    }
}