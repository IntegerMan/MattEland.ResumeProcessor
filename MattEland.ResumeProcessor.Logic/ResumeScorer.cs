using System;
using System.Linq;
using MattEland.ResumeProcessor.Models;

namespace MattEland.ResumeProcessor.Logic
{
    public class ResumeScorer
    {
        private readonly EmailNotifier _notifier;

        public ResumeScorer()
        {
            _notifier = new EmailNotifier();
        }

        public decimal EvaluateResumeForOpportunity(Resume resume, Opportunity opportunity)
        {
            decimal total = 0;

            total += ScoreYearsExperience(resume);

            if (opportunity.IsSufficientScoreFor(total))
            {
                _notifier.NotifyEmailAddressOfQualifiedForPosition(resume.EmailAddress, opportunity);
            }
            else
            {
                _notifier.NotifyEmailAddressOfDidNotMeet(resume.EmailAddress, opportunity);
            }

            return total;
        }

        private decimal ScoreYearsExperience(Resume resume)
        {
            return resume.Jobs.Sum(j => CalculateMonthsInJob(j) * 0.15m);
        }

        private int CalculateMonthsInJob(Job job)
        {
            var days = job.Finished.Date.Subtract(job.Started.Date).TotalDays;

            int numMonths = (int) Math.Round(days / 30);

            return numMonths;
        }
    }
}
