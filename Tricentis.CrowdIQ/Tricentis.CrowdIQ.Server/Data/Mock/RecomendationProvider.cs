﻿using System;
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
                IdentificationJavascript="typeof Ext != \"undefined\";"
            }
        };
        public List<RecommendationResponse> Recomendations
        {
            get { return recommendations; }
        } 
    }
}
