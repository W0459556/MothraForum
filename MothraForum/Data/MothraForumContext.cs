using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MothraForum.Models;

namespace MothraForum.Data
{
    public class MothraForumContext : DbContext
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
