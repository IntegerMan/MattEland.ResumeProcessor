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
    public class ResumesController : Controller
    {
        private readonly ResumeScorer _scorer;

        public ResumesController()
        {
            _scorer = new ResumeScorer();
        }

        [HttpPost]
        [Route("{resId:int}/scorer/{oppId:int}")]
        public ActionResult<decimal> ScoreResume(int resId, int oppId)
        {
            using (var context = new ResumeContext())
            {

                var resume = GetById(resId, context);
                if (resume == null)
                {
                    return NotFound("No resume exists with that ID or you do not have access to it.");
                }

                var opp = context.Opportunities.Include(o => o.DesiredSkills).FirstOrDefault(o => o.Id == oppId);
                if (opp == null)
                {
                    return NotFound("No opportunity exists with that ID or you do not have access to it.");
                }

                return Ok(_scorer.EvaluateResumeForOpportunity(resume, opp));
            }
        }

        [HttpGet]
        public ActionResult<List<Resume>> GetResumes()
        {
            using (var context = new ResumeContext())
            {
                InitResumesIfNeeded(context);

                return Ok(context.Resumes.Include(r => r.Educations).Include(r => r.Jobs).ToList());
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<Resume> GetResume(int id)
        {
            using (var context = new ResumeContext())
            {
                InitResumesIfNeeded(context);

                var match = GetById(id, context);

                if (match != null)
                {
                    return Ok(match);
                }

                return NotFound("No resume exists with that ID or you do not have access to it.");
            }
        }

        private static Resume GetById(int id, ResumeContext context) =>
            context.Resumes.Include(r => r.Educations)
                   .Include(r => r.Jobs)
                   .FirstOrDefault(r => r.Id == id);

        [HttpPost]
        public ActionResult<Resume> CreateResume(Resume opportunity)
        {
            if (opportunity == null) return new BadRequestResult();

            using (var context = new ResumeContext())
            {

                var result = context.Resumes.Add(opportunity);
                context.SaveChanges();

                // TODO: This really should return a full URL starting relative to the controller
                return Created($"/api/resumes/{result.Entity.Id}", result.Entity);
            }
        }

        [HttpPut("{id:int}")]
        [HttpPost("{id:int}")]
        public ActionResult<Opportunity> UpdateResume(int id, Resume resume)
        {
            if (resume == null || id != resume.Id) return new BadRequestResult();

            using (var context = new ResumeContext())
            {

                var existing = GetById(id, context);
                if (existing == null)
                {
                    return NotFound("No resume exists with that ID or you do not have access to it.");
                }

                existing.EmailAddress = resume.EmailAddress;
                existing.FirstName = resume.FirstName;
                existing.LastName = resume.LastName;
                existing.HomePostalCode = resume.HomePostalCode;
                existing.RequiresRelocation = resume.RequiresRelocation;
                existing.RequiresRemote = resume.RequiresRemote;
                existing.RequiresSponsorship = resume.RequiresSponsorship;

                // TODO: This should also update desired skills, potentially
                context.SaveChanges();

                return Ok(existing);
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult<Opportunity> DeleteOpportunity(int id)
        {
            using (var context = new ResumeContext())
            {
                InitResumesIfNeeded(context);

                var match = GetById(id, context);
                ;

                if (match != null)
                {
                    context.Resumes.Remove(match);
                    context.SaveChanges();

                    return Ok();
                }

                return NotFound("No resume exists with that ID or you do not have access to it.");
            }
        }

        private static void InitResumesIfNeeded(ResumeContext context)
        {
            // Add a sample opportunity if none is present
            if (!context.Resumes.Any())
            {
                context.Resumes.Add(new Resume
                {
                    FirstName = "Matt",
                    LastName = "Eland",
                    Educations = new List<Education>(),
                    EmailAddress = "Matt@GitHubSpam.com",
                    HomePostalCode = "50000",
                    RequiresRelocation = true,
                    RequiresRemote = false,
                    RequiresSponsorship = false
                });
                context.SaveChanges();
            }
        }
    }
}