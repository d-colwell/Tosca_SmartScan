using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tricentis.CrowdIQ.Controls
{
    public class WindowParameters
    {
        public IList<Customisation> Customisations { get; set; }
    }

    public class Customisation
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool Download { get; set; }
    }
}
