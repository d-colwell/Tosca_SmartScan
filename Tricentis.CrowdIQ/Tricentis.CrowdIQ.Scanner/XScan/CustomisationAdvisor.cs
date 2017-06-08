﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            IEnumerable<RecommendationResponse> recommendationResponses = null;
            List<RecommendationResponse> successfulRecommendations = null;

            String url = "http://localhost:53902/", urlParameters = "/api/recommendation/?engine=html";

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
            successfulRecommendations = recommendationResponses.ToList<RecommendationResponse>();
            foreach (RecommendationResponse rec in recommendationResponses)
            {
                String result = doc.EntryPoint.GetJavaScriptResult(rec.IdentificationJavascript);
                Boolean isValid = false;
                if (!Boolean.TryParse(result, out isValid))
                {
                    successfulRecommendations.RemoveAll(r => r.id == rec.id);
                }
            }
            #endregion

            #region Show recommendations (somehow)
            if(successfulRecommendations.Any())
            {
                ParameterizedThreadStart pts = new ParameterizedThreadStart(ThreadStart);
                Thread t = new Thread(ThreadStart);
                t.SetApartmentState(ApartmentState.STA);
                t.Start(null);
                t.Join();
            }
            #endregion
            return true;
        }
    private void ThreadStart(object target)
    {
        //if (app == null)
        //    app = new System.Windows.Application { ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown };
        var configWindow = new CrowdIQ.Controls.MainWindow();
        //app.Run(configWindow);
        configWindow.ShowDialog();
    }
}
}
