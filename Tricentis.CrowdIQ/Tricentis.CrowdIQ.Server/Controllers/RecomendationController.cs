using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tricentis.CrowdIQ.Server.Models.Recomendation;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Tricentis.CrowdIQ.Server.Controllers
{
    [Route("api/[controller]")]
    public class RecomendationController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<RecomendationResponse> Get(GetRecomendationsRequest request)
        {
            return Data.MockDataProvider.Instance.Recomendation.Recomendations.Where(x => x.engine == request.engine);
        }
    }
}
