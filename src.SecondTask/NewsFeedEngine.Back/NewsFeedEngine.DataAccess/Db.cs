using Microsoft.EntityFrameworkCore;
using NewsFeedEngine.Domain.Aggregates;

namespace NewsFeedEngine.DataAccess
{
    public sealed class Db : DbContext
    {
        public Db(DbContextOptions options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<ElementStatus> ElementStatuses => Set<ElementStatus>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<User>(new User());
            modelBuilder.ApplyConfiguration<Post>(new Post());
            modelBuilder.ApplyConfiguration<ElementStatus>(new ElementStatus());
        }
    }
}