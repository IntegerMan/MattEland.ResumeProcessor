using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MattEland.ResumeProcessor.Models
{
    public class Opportunity
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string Company { get; set; }

        public string PostalCode { get; set; }

        public IList<Skill> DesiredSkills { get; set; } = new List<Skill>();

        public bool IsSufficientScoreFor(decimal total)
        {
            return total > 50;
        }
    }
}