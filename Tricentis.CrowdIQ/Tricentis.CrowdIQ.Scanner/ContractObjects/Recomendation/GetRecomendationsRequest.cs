using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tricentis.CrowdIQ.Scanner.ContractObjects.Recomendation
{
    public class GetRecomendationsRequest
    {
        [JsonProperty(Required =Required.Always)]
        public string engine { get; set; }
    }
}
