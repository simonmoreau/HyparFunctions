using Elements;
using Elements.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LevelsFromEnvelope
{
      public static class LevelsFromEnvelope
    {
        /// <summary>
        /// The LevelsFromEnvelope function.
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A LevelsFromEnvelopeOutputs instance containing computed results and the model with any new elements.</returns>
        public static LevelsFromEnvelopeOutputs Execute(Dictionary<string, Model> inputModels, LevelsFromEnvelopeInputs input)
        {
            var envelopes = new List<Envelope>();
            inputModels.TryGetValue("Envelope", out var model);
            if (model == null || model.AllElementsOfType<Envelope>().Count() == 0)
            {
                throw new ArgumentException("No Envelope found.");
            }
            envelopes.AddRange(model.AllElementsOfType<Envelope>());
            var levelMaker = new LevelMaker(envelopes);
            
            // All the base levels
            var envolope = envelopes.First();
            int i = 0;
            foreach (double elevation in input.BaseLevels)
            {
                levelMaker.MakeLevel(envolope,elevation, $"Level {i.ToString()}");
            }
            var levelArea = 0.0;
            foreach (var lp in levelMaker.LevelPerimeters)
            {
                levelArea += lp.Area;
            }
            var output = new LevelsFromEnvelopeOutputs(levelMaker.Levels.Count(),
                                                     levelArea);
            output.Model.AddElements(levelMaker.Levels);
            output.Model.AddElements(levelMaker.LevelPerimeters);

                        var matl = BuiltInMaterials.Glass;
            matl.SpecularFactor = 0.0;
            matl.GlossinessFactor = 0.0;
            foreach (var item in levelMaker.LevelPerimeters)
            {
                output.Model.AddElement(new Panel(item.Perimeter, matl, new Transform(0.0, 0.0, item.Elevation),
                                        null, false, Guid.NewGuid(), ""));
            }
            
            return output;
        }
      }
}