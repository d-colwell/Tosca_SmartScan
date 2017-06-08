using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tricentis.Automation.Creation;
using Tricentis.Automation.Engines.Technicals.Html;
using Tricentis.Automation.XScan.Model;
using Tricentis.Automation.XScan.Result.Controller;
using Tricentis.Automation.XScan.Result.Tasks.Configs;
using Tricentis.Automation.XScan.Result.Tasks.Html;
using Tricentis.CrowdIQ.Scanner.ContractObjects.Recomendation;

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

            String url = "", urlParameters = "";

            // Retrieve information/hints
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                var dataObjects = response.Content.<IEnumerable<RecomendationResponse>>().Result;
                foreach (var d in dataObjects)
                {
                    Console.WriteLine("{0}", d.Name);
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }



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


            // Look for recommendations



            // Show recommendations (somehow)



            return false;
        }
    }
}
