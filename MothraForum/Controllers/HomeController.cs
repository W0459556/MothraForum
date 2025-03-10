using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MothraForum.Data;
using MothraForum.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly MothraForumContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(MothraForumContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var discussions = await _context.Discussions
            .Include(d => d.User)
            .Include(d => d.Votes)
            .Include(d => d.Comments)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();

        return View(discussions);
    }


    public async Task<IActionResult> DiscussionDetails(int id)
    {
        var discussion = await _context.Discussions
            .Include(d => d.User)
            .Include(d => d.Comments)
                .ThenInclude(c => c.User)
            .Include(d => d.Votes)
            .OrderByDescending(d => d.CreatedAt)
            .FirstOrDefaultAsync(d => d.DiscussionId == id);

        if (discussion == null)
        {
            return NotFound(); 
        }

        return View(discussion);
    }

    [HttpPost]
    public async Task<IActionResult> Vote(int id, int value)
    {
        var discussion = await _context.Discussions.Include(d => d.Votes).FirstOrDefaultAsync(d => d.DiscussionId == id);

        if (discussion != null)
        {
            var vote = new Vote
            {
                DiscussionId = id,
                Value = value
            };
            _context.Add(vote);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(DiscussionDetails), new { id });
    }

    public async Task<IActionResult> Profile(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var discussions = await _context.Discussions
            .Where(d => d.ApplicationUserId == user.Id)
            .Include(d => d.User)
            .Include(d => d.Votes)
            .Include(d => d.Comments)
            .ToListAsync();

        var model = new ProfileViewModel
        {
            User = user,
            Discussions = discussions
        };

        return View(model);
    }

}
