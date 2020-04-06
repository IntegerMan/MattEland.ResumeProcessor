using System.Collections.Generic;
using System.Linq;
using MattEland.ResumeProcessor.Logic;
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
        public ActionResult<List<Opportunity>> GetOpportunities()
        {
            using var context = new ResumeContext();
            InitOpportunitiesIfNeeded(context);

            return Ok(context.Opportunities.Include(o => o.DesiredSkills).ToList());
        }

        [HttpGet("{id:int}")]
        public ActionResult<Opportunity> GetOpportunity(int id)
        {
            using var context = new ResumeContext();
            InitOpportunitiesIfNeeded(context);

            var match = GetById(id, context);

            if (match != null)
            {
                return Ok(match);
            }

            return NotFound("No opportunity exists with that ID or you do not have access to it.");
        }

        private static Opportunity GetById(int id, ResumeContext context)
        {
            return context.Opportunities.Include(o => o.DesiredSkills).FirstOrDefault(o => o.Id == id);
        }

        [HttpPost]
        public ActionResult<Opportunity> CreateOpportunity(Opportunity opportunity)
        {
            if (opportunity == null) return new BadRequestResult();

            using var context = new ResumeContext();

            var result = context.Opportunities.Add(opportunity);
            context.SaveChanges();

            // TODO: This really should return a full URL starting relative to the controller
            return Created($"/api/opportunities/{result.Entity.Id}", result.Entity);
        }

        [HttpPut("{id:int}")]
        [HttpPost("{id:int}")]
        public ActionResult<Opportunity> UpdateOpportunity(int id, Opportunity opportunity)
        {
            if (opportunity == null || id != opportunity.Id) return new BadRequestResult();
            
            using var context = new ResumeContext();

            var existing = GetById(id, context);
            if (existing == null) return NotFound("No opportunity exists with that ID or you do not have access to it.");

            existing.Company = opportunity.Company;
            existing.JobTitle = opportunity.JobTitle;
            existing.PostalCode = opportunity.PostalCode;

            // TODO: This should also update desired skills, potentially
            context.SaveChanges();

            return Ok(existing);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Opportunity> DeleteOpportunity(int id)
        {
            using var context = new ResumeContext();
            InitOpportunitiesIfNeeded(context);

            var match = GetById(id, context); ;

            if (match != null)
            {
                context.Opportunities.Remove(match);
                context.SaveChanges();

                return Ok();
            }

            return NotFound("No opportunity exists with that ID or you do not have access to it.");
        }

        private static void InitOpportunitiesIfNeeded(ResumeContext context)
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
                        new Skill {ShortName = "Underwater Basket Weaving", Level = SkillLevel.Beginner}
                    }
                });
                context.SaveChanges();
            }
        }
    }

}