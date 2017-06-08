using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tricentis.CrowdIQ.Server.Models.Recomendation
{
    public class Recomendation
    {
        public Guid id { get; set; }
        public string engine { get; set; }
        public string customizationName { get; set; }
        public string IdentificationJavascript { get; set; }
    }
}
