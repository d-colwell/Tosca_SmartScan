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

        };
        public List<RecomendationResponse> Recomendations
        {
            get { return recomendations; }
        } 
    }
}
