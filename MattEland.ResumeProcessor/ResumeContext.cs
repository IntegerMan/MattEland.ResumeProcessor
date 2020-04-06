using MattEland.ResumeProcessor.Models;
using Microsoft.EntityFrameworkCore;

namespace MattEland.ResumeProcessor
{
    public class ResumeContext : DbContext
    {
        public ResumeContext() : base()
        {
        }

        public ResumeContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<Resume> Resumes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=ALFRED;Initial Catalog=Resumes;Integrated Security=True;Pooling=False");
        }
    }
}