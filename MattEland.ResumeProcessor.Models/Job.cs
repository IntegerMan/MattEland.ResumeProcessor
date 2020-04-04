using System;
using System.ComponentModel.DataAnnotations;

namespace MattEland.ResumeProcessor.Models
{
    public class Job
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string Company { get; set; }

        public DateTime Started { get; set; }

        public DateTime Finished { get; set; }

        public bool IsCurrentJob { get; set; }

        public bool WasFired { get; set; }
    }
}