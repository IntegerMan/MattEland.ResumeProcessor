using MattEland.ResumeProcessor.Models;

namespace MattEland.ResumeProcessor.Controllers
{
    public class ResumeEvaluationRequest
    {
        public Resume Resume { get; set; }
        public Opportunity Opportunity { get; set; }
    }
}