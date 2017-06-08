using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.Automation.Engines.Adapters;
using Tricentis.Automation.Engines.Adapters.Attributes;
using Tricentis.Automation.Engines.Adapters.Html;
using Tricentis.Automation.Engines.Technicals;
using Tricentis.Automation.Engines.Technicals.Html;
using Tricentis.Automation.XScan.Model;
using Tricentis.Automation.XScan.Result.Tasks.Html;

namespace Tricentis.CrowdIQ.Scanner
{
    [SupportedTechnical(typeof(IHtmlDocumentTechnical))]
    public class GenericAdapter : IHtmlAreaAdapter
    {
        public System.Drawing.PointF ActionPoint
        {
            get
            {
                IScanNode sn;
                HtmlMapDefaultIdsTask t;
                t.
                throw new NotImplementedException();
            }
        }

        public IAdapter Context
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public System.Drawing.RectangleF ControlArea
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DefaultName
        {
            get
            {
                return "Should be DEVExpress";
            }
        }

        public bool Enabled
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Focused
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool InteractiveElement
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSteerable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IHtmlMapAdapter Map
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ITechnical Technical
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Visible
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler Disposing;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public System.Drawing.RectangleF GetBorderWidths()
        {
            throw new NotImplementedException();
        }

        public System.Drawing.RectangleF GetClientArea()
        {
            throw new NotImplementedException();
        }

        public IGuiAdapter GetContext()
        {
            throw new NotImplementedException();
        }

        public System.Drawing.RectangleF GetControlArea(bool refresh)
        {
            throw new NotImplementedException();
        }

        public object GetProperty(string name, bool fallback)
        {
            throw new NotImplementedException();
        }

        public bool IsInViewPort()
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void ResetControlArea()
        {
            throw new NotImplementedException();
        }

        public void ScrollToVisible()
        {
            throw new NotImplementedException();
        }

        public void SetFocus()
        {
            throw new NotImplementedException();
        }

        public void SetProperty(string name, object value)
        {
            throw new NotImplementedException();
        }
    }
}
