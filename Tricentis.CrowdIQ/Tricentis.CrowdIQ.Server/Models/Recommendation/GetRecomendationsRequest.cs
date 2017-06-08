using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tricentis.CrowdIQ.Server.Models.Recommendation
{

    public class GetRecommendationsRequest
    {
        [JsonProperty(Required =Required.Always)]
        public string engine { get; set; }
    }
}
