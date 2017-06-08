using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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
    [SupportedScanResultTaskConfig(typeof(MapDefaultIdsTaskConfig))]
    public class CustomisationAdvisor : HtmlMapDefaultIdsTask
    {
        public CustomisationAdvisor(MapDefaultIdsTaskConfig taskConfig, IResultController controller, Validator validator) : base(taskConfig, controller, validator)
        {
            validator.AssertTrue(RecommendCustomisations(taskConfig.ResultNode, ref controller));
        }

        private bool RecommendCustomisations(IScanNode resultNode, ref IResultController controller)
        {
            IHtmlDocumentTechnical doc = ((ScanRepresentationNode)resultNode).Representation.Adapter.Technical as IHtmlDocumentTechnical;
            if (doc == null)
                return false;
            IEnumerable<RecommendationResponse> recommendationResponses = null;
            List<RecommendationResponse> successfulRecommendations = new List<RecommendationResponse>();

            String url = "http://localhost:50826/", urlParameters = "/api/recommendation/?engine=html";

            #region  Retrieve information/hints
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage response = client.GetAsync(urlParameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    String responseContent = response.Content.ReadAsStringAsync().Result;
                    recommendationResponses = JsonConvert.DeserializeObject<IEnumerable<RecommendationResponse>>(responseContent);
                }
                else
                {
                    // Alert user... or maybe do nothing
                    return false;
                }
            }
            catch
            {
                return false;
            }

            #endregion

            #region Look for recommendations
            foreach (RecommendationResponse rec in recommendationResponses)
            {
                String result = doc.EntryPoint.GetJavaScriptResult(rec.IdentificationJavascript);
                Boolean isValid;
                Boolean.TryParse(result, out isValid);
                if (isValid)
                {
                    successfulRecommendations.Add(rec);
                }
            }
            #endregion

            #region Show recommendations (somehow)
            if (successfulRecommendations.Any())
            {
                ParameterizedThreadStart pts = new ParameterizedThreadStart(ThreadStart);
                Thread t = new Thread(ThreadStart);
                t.SetApartmentState(ApartmentState.STA);
                Controls.WindowParameters winParam = new Controls.WindowParameters
                {
                    Customisations = successfulRecommendations.Select(
                        r => new Controls.Customisation
                        {
                            ID = r.id,
                            Name = r.customizationName,
                            Download = false
                        }).ToList()
                };
                t.Start(winParam);
                t.Join();

                
                t.DisableComObjectEagerCleanup();   // Prevent GC from attempting to clean up
                t.Abort();                          // Abort and let thread handle termination of itself
                GC.Collect();                       // Now call GC
            }
            #endregion
            return true;
        }

        private void ThreadStart(object target)
        {
            //if (app == null)
            //    app = new System.Windows.Application { ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown };
            var configWindow = new CrowdIQ.Controls.MainWindow(target as Controls.WindowParameters);
            //app.Run(configWindow);
            configWindow.ShowDialog();
        }
    }
}
