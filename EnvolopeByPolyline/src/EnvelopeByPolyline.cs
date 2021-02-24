using Elements;
using Elements.Geometry;
using System.Collections.Generic;

namespace EnvelopeByPolyline
{
      public static class EnvelopeByPolyline
    {
        /// <summary>
        /// Generate a volume of a given width 
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A EnvelopeByPolylineOutputs instance containing computed results and the model with any new elements.</returns>
        public static EnvelopeByPolylineOutputs Execute(Dictionary<string, Model> inputModels, EnvelopeByPolylineInputs input)
        {
             /// Your code here.
            var height = 1.0;
            var volume = input.Length * input.Width * height;
            var output = new EnvelopeByPolylineOutputs(volume);
            var rectangle = Polygon.Rectangle(input.Length, input.Width);
            var mass = new Mass(rectangle, height);
            output.Model.AddElement(mass);
            return output;
        }
      }
}