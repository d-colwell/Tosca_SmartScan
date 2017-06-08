using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tricentis.CrowdIQ.Server.Models.Recommendation;

namespace Tricentis.CrowdIQ.Server.Data.Mock
{
    public class RecommendationProvider
    {
        private List<RecommendationResponse> recommendations = new List<RecommendationResponse>
        {
            new RecommendationResponse
            {
                customizationName="Extended JavaScript UI",
                engine="html",
                id = Guid.NewGuid(),
                IdentificationJavascript="return typeof Ext != \"undefined\";",
                MaxToscaVersion = "10.2",
                MinToscaVersion = "10.0",
                Version = "0.9"
            },
            new RecommendationResponse
            {
                customizationName="Extended JavaScript UI V2",
                engine="html",
                id = Guid.NewGuid(),
                IdentificationJavascript="return true;",
                MaxToscaVersion = "10.2",
                MinToscaVersion = "10.1",
                Version = "0.9"
            }
        };
        public List<RecommendationResponse> Recomendations
        {
            get { return recommendations; }
        } 
    }
}
