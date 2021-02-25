using Elements;
using Elements.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var envelopes = new List<Envelope>();
            inputModels.TryGetValue("Envelope", out var model);
            if (model == null || model.AllElementsOfType<Envelope>().Count() == 0)
            {
                throw new ArgumentException("No Envelope found.");
            }
            envelopes.AddRange(model.AllElementsOfType<Envelope>());
            var levelMaker = new LevelMaker(envelopes, input.BaseLevels, "Level");

            var levelArea = 0.0;
            foreach (var lp in levelMaker.LevelPerimeters)
            {
                levelArea += lp.Area;
            }
            var output = new LevelsFromEnvelopesOutputs(levelMaker.Levels.Count(),
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