using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tricentis.CrowdIQ.Server.Models.Recomendation;
using Microsoft.AspNetCore.Hosting;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Tricentis.CrowdIQ.Server.Controllers
{
    [Route("api/[controller]")]
    public class RecommendationController : Controller
    {
        IHostingEnvironment host;
        public RecommendationController(IHostingEnvironment host)
        {
            this.host = host;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<RecomendationResponse> Get(GetRecomendationsRequest request)
        {
            var recomendations = Data.MockDataProvider.Instance.Recomendation.Recomendations.Where(x => x.engine == request.engine).ToList();
            return recomendations;
        }

        [HttpGet]
        [Route("{id}")]
        public FileContentResult Get(Guid id)
        {
            var recomendation = Data.MockDataProvider.Instance.Recomendation.Recomendations.FirstOrDefault(x => x.id == id);
            if (recomendation == null)
            {
                NotFound();
                return null;
            }
            return File(System.IO.File.ReadAllBytes(host.WebRootFileProvider.GetFileInfo($"Customisations\\{recomendation.customizationName}.dll").PhysicalPath), "application/x-msdownload");
        }
    }
}
