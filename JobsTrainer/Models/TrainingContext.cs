using Microsoft.EntityFrameworkCore;

namespace JobsTrainer.Models
{
    public class TrainingContext : DbContext
    {
        public TrainingContext(DbContextOptions<TrainingContext> options) : base(options)
        {
        }

        public DbSet<TrainJob> TrainJobs { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Keyword> Keywords { get; set; }

        public DbSet<Skill> Skills { get; set; }

        public DbSet<LmiaInfo> LmiaInfos  { get; set; }
    }
}
