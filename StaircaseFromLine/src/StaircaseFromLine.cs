using Elements;
using Elements.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StaircaseFromLine
{
    public static class StaircaseFromLine
    {
        /// <summary>
        /// Create a staircase of a given width aligned along a line
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A StaircaseFromLineOutputs instance containing computed results and the model with any new elements.</returns>
        public static StaircaseFromLineOutputs Execute(Dictionary<string, Model> inputModels, StaircaseFromLineInputs input)
        {
            if (input.StaircaseAxe == null)
            {
                throw new ArgumentException("Please draw the axe of the staircase.");
            }

            var levels = new List<Level>();
            inputModels.TryGetValue("Levels", out var model);
            if (model == null || model.AllElementsOfType<Level>().Count() == 0)
            {
                throw new ArgumentException("No Level found.");
            }

            levels.AddRange(model.AllElementsOfType<Level>());

            levels = levels.OrderBy(l => l.Elevation).ToList();



            //Build a polygon aligned with the line
            Line stairCaseAxe = input.StaircaseAxe;
            // stairCaseAxe.Direction


            double uniteDePassage = input.Width;

            if (uniteDePassage == 0)
            {
                throw new ArgumentException("Can't create a stair with no width.");
            }

            double runWidth = 0;
            double thread = 0.3;
            double maxRiser = 0.14;
            double levelHeight = 3.5;

            if (uniteDePassage == 1) { runWidth = 0.9; }
            else if (uniteDePassage == 2) { runWidth = 1.4; }
            else { runWidth = 0.6 * uniteDePassage; }

            double wallWidth = 0.2;
            double landingWidth = 2.5;
            double realLandingWidth = Math.Max(landingWidth, runWidth);
            double runLengh = Math.Ceiling(Math.Ceiling(levelHeight / maxRiser) / 2) * thread;
            double totalLenght = wallWidth * 2 + runWidth + realLandingWidth + runLengh;
            double totalWidth = wallWidth + runWidth + wallWidth + runWidth + wallWidth;
            
            Vector3 widthDirection = Vector3.ZAxis.Cross(stairCaseAxe.Direction()).Negate();
            Vector3 footprintWidth = widthDirection * totalWidth;
            Vector3 footprintLenght = stairCaseAxe.Direction() * totalLenght;
            Polygon staircaseFoorprint = new Polygon(new List<Vector3>{
                stairCaseAxe.Start,
                stairCaseAxe.Start + footprintLenght,
                stairCaseAxe.Start + footprintLenght + footprintWidth,
                stairCaseAxe.Start + footprintWidth,
            });

            var envelopeMatl = new Material("envelope", new Color(0.3, 0.7, 0.7, 0.2), 0.0f, 0.0f);
            var stairEnclosures = new List<StairEnclosure>();
            var stairs = new List<Stair>();

            var extrude = new Elements.Geometry.Solids.Extrude(staircaseFoorprint, levels.Last().Elevation, Vector3.ZAxis, false);
            var geomRep = new Representation(new List<Elements.Geometry.Solids.SolidOperation>() { extrude });
            stairEnclosures.Add(new StairEnclosure(staircaseFoorprint,Vector3.ZAxis, 0.0, 0.0, levels.Last().Elevation, staircaseFoorprint.Area(),"1",
                          new Transform(), envelopeMatl, geomRep, false, Guid.NewGuid(), ""));

            
            Vector3 stairPathStart = stairCaseAxe.Start + widthDirection * (wallWidth + runWidth /2 );
            Vector3 secondFlightPathStart = stairPathStart + stairCaseAxe.Direction()* runLengh + widthDirection*(runWidth+wallWidth);
            List<Line> stairPaths = new List<Line>{
                new Line(stairPathStart,stairPathStart + stairCaseAxe.Direction()* runLengh),
                new Line(secondFlightPathStart,secondFlightPathStart + stairCaseAxe.Direction().Negate()* runLengh)
            };

            StairMaker stairMaker = new StairMaker(maxRiser,thread,runWidth, stairPaths, levels);

            var output = new StaircaseFromLineOutputs(2);

            output.Model.AddElements(stairEnclosures);
            output.Model.AddElements(stairMaker.Stairs);
            return output;
        }
    }
}