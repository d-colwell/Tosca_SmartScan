﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tricentis.CrowdIQ.Scanner.ContractObjects.Recomendation
{
    public class RecomendationResponse
    {
        public Guid id { get; set; }
        public string engine { get; set; }
        public string customizationName { get; set; }
        public string IdentificationJavascript { get; set; }
    }
}