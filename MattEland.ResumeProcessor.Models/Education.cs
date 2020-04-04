using System.ComponentModel.DataAnnotations;

namespace MattEland.ResumeProcessor.Models
{
    public class Education
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string School { get; set; }

        [Required]
        public EducationLevel Level { get; set; }
    }
}