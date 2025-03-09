using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MothraForum.Models;

namespace MothraForum.Data
{
    public class MothraForumContext : IdentityDbContext
    {
        public MothraForumContext(DbContextOptions<MothraForumContext> options)
            : base(options)
        {
        }

        public DbSet<MothraForum.Models.Discussion> Discussions { get; set; } = default!;
        public DbSet<MothraForum.Models.Comment> Comments { get; set; } = default!;
        public DbSet<MothraForum.Models.Vote> Votes { get; set; } = default!;
    }
}
