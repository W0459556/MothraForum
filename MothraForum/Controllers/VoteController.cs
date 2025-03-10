using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MothraForum.Data;
using MothraForum.Models;

namespace MothraForum.Controllers
{
    public class VoteController : Controller
    {
        private readonly MothraForumContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public VoteController(MothraForumContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(int discussionId, int value)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized(); 
            }

            var discussion = await _context.Discussions.Include(d => d.Votes)
                                                       .FirstOrDefaultAsync(d => d.DiscussionId == discussionId);

            if (discussion == null)
            {
                return NotFound();
            }

            var existingVote = await _context.Votes
                .FirstOrDefaultAsync(v => v.DiscussionId == discussionId && v.ApplicationUserId == userId);

            if (existingVote != null)
            {
                if (existingVote.Value == value)
                {
                    _context.Votes.Remove(existingVote);
                }
                else
                {
                    existingVote.Value = value;
                    _context.Votes.Update(existingVote);
                }
            }
            else
            {
                var vote = new Vote
                {
                    DiscussionId = discussionId,
                    ApplicationUserId = userId,
                    Value = value
                };
                _context.Votes.Add(vote);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home", new { id = discussionId });
        }
    }
}
