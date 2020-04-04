using System.Collections.Generic;
using System.Linq;
using MattEland.ResumeProcessor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                // Add a sample opportunity if none is present
                if (!context.Opportunities.Any())
                {
                    context.Opportunities.Add(new Opportunity()
                    {
                        Company = "FakeCorp",
                        PostalCode = "43081",
                        JobTitle = "Basket Engineer",
                        DesiredSkills = new List<Skill>
                        {
                            new Skill { ShortName = "Underwater Basket Weaving", Level = SkillLevel.Beginner }
                        }
                    });
                    context.SaveChanges();
                }

                return context.Opportunities.Include(o => o.DesiredSkills).ToList();
            }
        }
    }
}