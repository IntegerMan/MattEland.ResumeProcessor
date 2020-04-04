using System.Collections.Generic;
using System.Linq;
using MattEland.ResumeProcessor.Models;
using Microsoft.AspNetCore.Mvc;

namespace MattEland.ResumeProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpportunitiesController : Controller
    {
        [HttpGet]
        public List<Opportunity> GetOpportunities()
        {
            using (var context = new ResumeContext())
            {
                return context.Opportunities.ToList();
            }
        }
    }
}