using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tricentis.CrowdIQ.Server.Models.Recomendation;

namespace Tricentis.CrowdIQ.Server.Data.Mock
{
    public class RecomendationProvider
    {
        private List<Recomendation> recomendations = new List<Recomendation>
        {

        };
        public List<Recomendation> Recomendations
        {
            get { return recomendations; }
        } 
    }
}
