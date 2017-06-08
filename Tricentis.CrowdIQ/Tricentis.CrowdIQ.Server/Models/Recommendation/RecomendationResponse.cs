using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tricentis.CrowdIQ.Server.Models.Recommendation
{
    public class RecommendationResponse
    {
        public Guid id { get; set; }
        public string engine { get; set; }
        public string customizationName { get; set; }
        public string IdentificationJavascript { get; set; }

        public string MinToscaVersion { get; set; }
        public string MaxToscaVersion { get; set; }
        public string Version { get; set; }
    }
}
