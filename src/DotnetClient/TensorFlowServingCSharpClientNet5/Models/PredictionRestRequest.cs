using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TensorFlowServingCSharpClientNet5.Models
{
    public class PredictionRestRequest
    {
        [JsonProperty("signature_name")]
        public string SignatureName { get; set; }

        [JsonProperty("instances")]
        public List<float> Instances { get; set; }
    }
}
