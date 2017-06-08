using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
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
            validator.AssertTrue(RecommendCustomisations(taskConfig.ResultNode, controller));
        }
        private bool KEEP_HASTLING = true;
        private String host = @"http://localhost:50826/";

        private bool RecommendCustomisations(IScanNode resultNode, IResultController controller)
        {
            IHtmlDocumentTechnical doc = ((ScanRepresentationNode)resultNode).Representation.Adapter.Technical as IHtmlDocumentTechnical;
            if (doc == null)
            {
                return false;
            }

            string tricentisHomePath = Environment.ExpandEnvironmentVariables("%tricentis_home%");
            string registerFile = Path.Combine(tricentisHomePath, Globals.REGISTER_FILE);
            if (!File.Exists(registerFile))
            {
                RecommendationRegister tempRegister = new RecommendationRegister
                {
                    InstalledCustomisations = new List<InstalledCustomisation>(),
                    RecommendationsRegister = new List<RecommendationRecord>()
                };
                File.WriteAllText(registerFile, JsonConvert.SerializeObject(tempRegister));
            }
            RecommendationRegister register = JsonConvert.DeserializeObject<RecommendationRegister>(File.ReadAllText(registerFile));

            var allRecommendations = GetRecomendations("html");
            var notYetInstalled = allRecommendations.Where(r => !register.InstalledCustomisations.Any(ic => ic.ID == r.id));
            var outdatedRecomendations = allRecommendations
                .Select(nr => new { Current = register.InstalledCustomisations.FirstOrDefault(x => x.ID == nr.id), New = nr })
                .Where(c => c.Current != null && c.New != null && string.Compare(c.Current.Version, c.New.Version) > 0);

            var uri = new Uri(doc.Url);
            string pageHost = uri.Host;

            bool recommendationHasBeenMadeBefore = HasRequestBeenMadeBefore(allRecommendations, register, pageHost,!KEEP_HASTLING);
            if (recommendationHasBeenMadeBefore)
                return false;

            var validRecommendations = notYetInstalled.Where(x => IsValidForDoc(x, doc));

            if (!validRecommendations.Any())
                return false;

            ParameterizedThreadStart pts = new ParameterizedThreadStart(ThreadStart);
            Thread t = new Thread(ThreadStart);
            t.SetApartmentState(ApartmentState.STA);
            Controls.WindowParameters winParam = new Controls.WindowParameters
            {
                Customisations = validRecommendations.Select(
                    r => new Controls.Customisation
                    {
                        ID = r.id,
                        Name = r.customizationName,
                        Download = false
                    }).ToList()
            };
            t.Start(winParam);
            t.Join();
            bool hasBeenDownloaded = false;
            foreach (var param in winParam.Customisations.Where(x => x.Download))
            {
                string file = DownloadCustomisation(param.ID, param.Name);
                register.InstalledCustomisations.Add(new InstalledCustomisation
                {
                    FilePath = file,
                    ID = param.ID,
                    InstallDate = DateTime.Now,
                    MaxSupportedToscaVersion = param.MaxToscaVersion,
                    MinSupportedToscaVersion = param.MinToscaVersion,
                    Version = param.Version
                });
                hasBeenDownloaded = true;
            }
            if (hasBeenDownloaded)
                controller.ShowInfoMessage("New customisations have been downloaded. Please restart Tosca");
            File.WriteAllText(registerFile, JsonConvert.SerializeObject(register));
            return false;
        }

        private bool HasRequestBeenMadeBefore(IEnumerable<RecommendationResponse> recommendations, RecommendationRegister register, string currentHost, bool addIfNotMade = false)
        {
            string jsonValue = JsonConvert.SerializeObject(recommendations);
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(jsonValue));
            }
            string hashString = Encoding.Default.GetString(hash);
            var recommendation = register.RecommendationsRegister.FirstOrDefault(x => x.PageURL == currentHost && x.RecommendationHash == hashString);
            if (recommendation != null)
            {
                return true;
            }
            if (addIfNotMade)
                register.RecommendationsRegister.Add(new RecommendationRecord
                {
                    DateRecomended = DateTime.Now,
                    PageURL = currentHost,
                    RecommendationHash = hashString
                });
            return false;
        }
        private IEnumerable<RecommendationResponse> GetRecomendations(string engine)
        {
            IEnumerable<RecommendationResponse> recommendations;
            String url = "http://localhost:50826/", urlParameters = $"/api/recommendation/?engine={engine}";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                HttpResponseMessage response = client.GetAsync(urlParameters).Result;
                if (response.IsSuccessStatusCode)
                {
                    String responseContent = response.Content.ReadAsStringAsync().Result;
                    recommendations = JsonConvert.DeserializeObject<IEnumerable<RecommendationResponse>>(responseContent);
                    return recommendations;
                }
                else
                {   // Alert user... or maybe do nothing and pretend nothing happened                   
                }
            }
            catch (Exception)
            {
                //NOM NOM NOM 
                //tasty exceptions
            }
            return new List<RecommendationResponse>();
        }
        private string DownloadCustomisation(Guid id, string name)
        {
            var uri = new Uri(host + $"/api/recommendation/{id}");

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
            return Path.Combine(directory, fileName + ".dll");
        }
        private bool IsValidForDoc(RecommendationResponse recommendation, IHtmlDocumentTechnical doc)
        {
            try
            {
                String result = doc.EntryPoint.GetJavaScriptResult(recommendation.IdentificationJavascript);
                Boolean isValid;
                if (!Boolean.TryParse(result, out isValid))
                    return false;
                return isValid;
            }
            catch (Exception)
            {
                return false;
            }

        }
        private void ThreadStart(object target)
        {
            var configWindow = new CrowdIQ.Controls.MainWindow(target as Controls.WindowParameters);
            var app = new System.Windows.Application();
            app.Run(configWindow);
        }
    }
}
