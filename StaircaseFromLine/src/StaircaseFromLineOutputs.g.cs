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

namespace StaircaseFromLine
{
    public class StaircaseFromLineOutputs: ResultsBase
    {
		/// <summary>
		/// The number of stair flights
		/// </summary>
		[JsonProperty("Number of flight")]
		public double NumberOfFlight {get;}



        /// <summary>
        /// Construct a StaircaseFromLineOutputs with default inputs.
        /// This should be used for testing only.
        /// </summary>
        public StaircaseFromLineOutputs() : base()
        {

        }


        /// <summary>
        /// Construct a StaircaseFromLineOutputs specifying all inputs.
        /// </summary>
        /// <returns></returns>
        [JsonConstructor]
        public StaircaseFromLineOutputs(double numberOfFlight): base()
        {
			this.NumberOfFlight = numberOfFlight;

		}

		public override string ToString()
		{
			var json = JsonConvert.SerializeObject(this);
			return json;
		}
	}
}