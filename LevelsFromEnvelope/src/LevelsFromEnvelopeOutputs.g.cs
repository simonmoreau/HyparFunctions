// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar init'.
// DO NOT EDIT THIS FILE.

using Elements;
using Elements.GeoJSON;
using Elements.Geometry;
using Hypar.Functions;
using Hypar.Functions.Execution;
using Hypar.Functions.Execution.AWS;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace LevelsFromEnvelope
{
    public class LevelsFromEnvelopeOutputs: ResultsBase
    {
		/// <summary>
		/// Total number of volume
		/// </summary>
		[JsonProperty("Level Quantity")]
		public double LevelQuantity {get;}

		/// <summary>
		/// Total Level Area
		/// </summary>
		[JsonProperty("Total Level Area")]
		public double TotalLevelArea {get;}



        /// <summary>
        /// Construct a LevelsFromEnvelopeOutputs with default inputs.
        /// This should be used for testing only.
        /// </summary>
        public LevelsFromEnvelopeOutputs() : base()
        {

        }


        /// <summary>
        /// Construct a LevelsFromEnvelopeOutputs specifying all inputs.
        /// </summary>
        /// <returns></returns>
        [JsonConstructor]
        public LevelsFromEnvelopeOutputs(double levelQuantity, double totalLevelArea): base()
        {
			this.LevelQuantity = levelQuantity;
			this.TotalLevelArea = totalLevelArea;

		}

		public override string ToString()
		{
			var json = JsonConvert.SerializeObject(this);
			return json;
		}
	}
}