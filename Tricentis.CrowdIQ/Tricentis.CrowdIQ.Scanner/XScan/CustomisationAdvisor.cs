using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Technicals.Html;
using Tricentis.Automation.XScan.Model;
using Tricentis.Automation.XScan.Result.Controller;
using Tricentis.Automation.XScan.Result.Tasks.Configs;
using Tricentis.Automation.XScan.Result.Tasks.Html;

namespace Tricentis.CrowdIQ.Scanner.XScan
{
    [SupportedScanResultTaskConfig(typeof(DefaultScanTaskConfig))]
    public class CustomisationAdvisor : HtmlMapDefaultIdsTask
    {
        public CustomisationAdvisor(MapDefaultIdsTaskConfig taskConfig, IResultController controller, Validator validator) : base(taskConfig, controller, validator)
        {
            validator.AssertTrue(RecommendCustomisations(taskConfig.ResultNode));
        }

        private bool RecommendCustomisations(IScanNode resultNode)
        {
            IHtmlDocumentTechnical doc;
            ScanRepresentationNode srNode = resultNode as ScanRepresentationNode;

            // Only applicable for Document object - so that it does not repeat for every element
            if (srNode != null && srNode.Representation != null && srNode.Representation.Adapter != null)
            {
                try
                {
                    doc = srNode.Representation.Adapter.Technical as IHtmlDocumentTechnical;
                }
                catch
                {
                    return false;
                }
            }

            // Retrieve information/hints

            #region Body Class

            
            #endregion


            // Look for recommendations



            // Show recommendations (somehow)



            return false;
        }
    }
}
