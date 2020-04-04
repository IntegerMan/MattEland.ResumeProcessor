using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MattEland.ResumeProcessor.Logic;
using MattEland.ResumeProcessor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MattEland.ResumeProcessor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumesController : ControllerBase
    {
        private readonly ResumeScorer _scorer;

        public ResumesController(ResumeScorer scorer)
        {
            _scorer = scorer;
        }

        [HttpPost]
        [Route("Scorer")]
        public decimal ScoreResume(ResumeEvaluationRequest request)
        {
            return _scorer.EvaluateResumeForOpportunity(request.Resume, request.Opportunity);
        }
    }

    public class ResumeEvaluationRequest
    {
        public Resume Resume { get; set; }
        public Opportunity Opportunity { get; set; }
    }
}