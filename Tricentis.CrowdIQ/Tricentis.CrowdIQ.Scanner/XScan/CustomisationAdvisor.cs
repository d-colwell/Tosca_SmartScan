using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            validator.AssertTrue(RecommendCustomisations(taskConfig.ResultNode));
        }

        private bool RecommendCustomisations(IScanNode resultNode)
        {
            IHtmlDocumentTechnical doc = ((ScanRepresentationNode)resultNode).Representation.Adapter.Technical as IHtmlDocumentTechnical;
            if (doc == null)
                return false;
            IEnumerable<RecommendationResponse> recommendationResponses = null;
            List<RecommendationResponse> successfulRecommendations = new List<RecommendationResponse>();

            #region  Retrieve information/hints
            String url = "http://localhost:50826/", urlParameters = "/api/recommendation/?engine=html";
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
                {   // Alert user... or maybe do nothing and pretend nothing happened                    
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

                foreach (var param in winParam.Customisations.Where(x=>x.Download))
                {
                    DownloadCustomisation(param.ID, param.Name);
                }
                controller.ShowInfoMessage("New customisations have been downloaded. Please restart Tosca");
                t.DisableComObjectEagerCleanup();   // Prevent GC from attempting to clean up
                t.Abort();                          // Abort and let thread handle termination of itself
                GC.Collect();                       // Now call GC



            }
            #endregion
            return true;
        }

        private void DownloadCustomisation(Guid id, string name)
        {
            var uri = new Uri($"http://localhost:50826/api/recommendation/{id}");

            HttpClient client = new HttpClient();
            var result = client.GetAsync(uri).Result;
            byte[] btyeContent = result.Content.ReadAsByteArrayAsync().Result;
            string directory = Environment.ExpandEnvironmentVariables("%tricentis_home%");
            string fileName = name.Replace(" ", "");

            if (File.Exists(Path.Combine(directory, fileName + ".dll")))
            {
                fileName = $"{fileName}-NEW";
                if (File.Exists(Path.Combine(directory, fileName + ".dll")))
                    File.Delete(Path.Combine(directory, fileName + ".dll"));
            }
            File.WriteAllBytes(Path.Combine(directory, fileName + ".dll"), btyeContent);
        }

        private void ThreadStart(object target)
        {
            var configWindow = new CrowdIQ.Controls.MainWindow(target as Controls.WindowParameters);
            configWindow.ShowDialog();
        }
    }
}
