using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tricentis.CrowdIQ.Server.Models.Recomendation;

namespace Tricentis.CrowdIQ.Server.Data.Mock
{
    public class RecomendationProvider
    {
        private List<RecomendationResponse> recomendations = new List<RecomendationResponse>
        {
            new RecomendationResponse
            {
                customizationName="Extended JavaScript UI",
                engine="html",
                id = Guid.NewGuid(),
                IdentificationJavascript="some JS here"
            }
        };
        public List<RecomendationResponse> Recomendations
        {
            get { return recomendations; }
        } 
    }
}
