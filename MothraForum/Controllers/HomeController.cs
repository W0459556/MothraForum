using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MothraForum.Data;
using MothraForum.Models;

public class HomeController : Controller
{
    private readonly MothraForumContext _context;

    public HomeController(MothraForumContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var discussions = await _context.Discussions
                                        .Include(d => d.Votes)
                                        .Include(d => d.Comments)
                                        .OrderByDescending(d => d.CreatedAt)
                                        .ToListAsync();

        return View(discussions);
    }

    public async Task<IActionResult> DiscussionDetails(int id)
    {
        var discussion = await _context.Discussions
                                       .Include(d => d.Votes)
                                       .Include(d => d.Comments)
                                       .OrderByDescending(d => d.CreatedAt)
                                       .FirstOrDefaultAsync(d => d.DiscussionId == id);

        if (discussion == null)
        {
            return NotFound();
        }

        return View(discussion);
    }
}
