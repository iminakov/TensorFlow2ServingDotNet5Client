using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TensorFlowServingCSharpClientNet5.Models
{
    public class PredictionRestResponse
    {
        [JsonProperty("predictions")]
        public float[][][] Predictions { get; set; }
    }
}
