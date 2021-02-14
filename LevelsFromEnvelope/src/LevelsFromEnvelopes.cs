using Elements;
using Elements.Geometry;
using System.Collections.Generic;

namespace LevelsFromEnvelopes
{
      public static class LevelsFromEnvelopes
    {
        /// <summary>
        /// Create levels in every envelopes in the project.
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A LevelsFromEnvelopesOutputs instance containing computed results and the model with any new elements.</returns>
        public static LevelsFromEnvelopesOutputs Execute(Dictionary<string, Model> inputModels, LevelsFromEnvelopesInputs input)
        {
             /// Your code here.
            var height = 1.0;
            var volume = input.Length * input.Width * height;
            var output = new LevelsFromEnvelopesOutputs(volume);
            var rectangle = Polygon.Rectangle(input.Length, input.Width);
            var mass = new Mass(rectangle, height);
            output.Model.AddElement(mass);
            return output;
        }
      }
}