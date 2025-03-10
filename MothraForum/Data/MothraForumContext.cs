using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MothraForum.Models;

namespace MothraForum.Data
{
    public class MothraForumContext : IdentityDbContext<ApplicationUser>
    {
        public MothraForumContext(DbContextOptions<MothraForumContext> options)
            : base(options)
        {
        }

        public DbSet<Discussion> Discussions { get; set; } = default!;
        public DbSet<Comment> Comments { get; set; } = default!;
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.DiscussionId, v.ApplicationUserId })
                .IsUnique();
        }
    }
}
