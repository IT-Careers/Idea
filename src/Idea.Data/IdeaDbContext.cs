using Idea.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Idea.Data
{
    public class IdeaDbContext : DbContext
    {
        public virtual DbSet<IdeaUser> Users { get; set; }

        public IdeaDbContext(DbContextOptions<IdeaDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
