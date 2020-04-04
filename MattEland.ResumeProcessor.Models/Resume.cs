using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MattEland.ResumeProcessor.Models
{
    public class Resume
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string HomePostalCode { get; set; }

        public bool RequiresRemote { get; set; }
        public bool RequiresSponsorship { get; set; }
        public bool RequiresRelocation { get; set; }

        public IList<Job> Jobs { get; set; } = new List<Job>();
        public IList<Education> Educations { get; set; } = new List<Education>();
    }
}
