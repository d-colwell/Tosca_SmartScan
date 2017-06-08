using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tricentis.CrowdIQ.Scanner
{
    public class RecommendationRegister
    {
        public List<InstalledCustomisation> InstalledCustomisations { get; set; }
        public List<RecommendationRecord> RecommendationsRegister { get; set; }
    }

    public class InstalledCustomisation
    {
        public Guid ID { get; set; }
        public DateTime InstallDate { get; set; }
        public string MinSupportedToscaVersion { get; set; }
        public string MaxSupportedToscaVersion { get; set; }
        public string Version { get; set; }
        public string FilePath { get; set; }
    }

    public class RecommendationRecord
    {
        public string PageURL { get; set; }
        public string RecommendationHash { get; set; }
        public DateTime DateRecomended { get; set; }
    }
}
