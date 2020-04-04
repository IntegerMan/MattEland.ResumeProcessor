using System.ComponentModel.DataAnnotations;

namespace MattEland.ResumeProcessor.Models
{
    public class Skill
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string ShortName { get; set; }

        public SkillLevel Level { get; set; }
    }
}