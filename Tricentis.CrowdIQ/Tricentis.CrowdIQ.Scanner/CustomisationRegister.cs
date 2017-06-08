using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tricentis.CrowdIQ.Scanner
{
    public class RecommendationRegister
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<InstalledCustomisation> InstalledCustomisations { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<RecommendationRecord> RecommendationsRegister { get; set; }
    }

    public class InstalledCustomisation
    {
        public Guid ID { get; set; }
        public DateTime InstallDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MinSupportedToscaVersion { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MaxSupportedToscaVersion { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Version { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FilePath { get; set; }
    }

    public class RecommendationRecord
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PageURL { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RecommendationHash { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateRecomended { get; set; }
    }
}
