using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MothraForum.Data;
using MothraForum.Models;

[Authorize] 
public class CommentController : Controller
{
    private readonly MothraForumContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CommentController(MothraForumContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Create(int discussionId)
    {
        ViewData["DiscussionId"] = discussionId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddComment(int discussionId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return RedirectToAction("DiscussionDetails", "Home", new { id = discussionId });
        }

        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var comment = new Comment
        {
            DiscussionId = discussionId,
            ApplicationUserId = userId,
            Content = content,
            CreatedAt = DateTime.Now
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("DiscussionDetails", "Home", new { id = discussionId });
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var comment = await _context.Comments.FindAsync(id);
        if (comment == null) return NotFound();

        var userId = _userManager.GetUserId(User);
        if (comment.ApplicationUserId != userId)
        {
            return Forbid();
        }

        return View(comment);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null) return NotFound();

        var userId = _userManager.GetUserId(User);
        if (comment.ApplicationUserId != userId)
        {
            return Forbid();
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }
}
