using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MothraForum.Data;
using MothraForum.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[Authorize] 
public class DiscussionController : Controller
{
    private readonly MothraForumContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public DiscussionController(MothraForumContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("Title,Content,ImageFile")] Discussion discussion)
    {
        if (ModelState.IsValid)
        {
            var userId = _userManager.GetUserId(User);
            discussion.ApplicationUserId = userId;
            discussion.CreatedAt = DateTime.Now;

            if (discussion.ImageFile != null)
            {
                var fileName = Path.GetFileName(discussion.ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await discussion.ImageFile.CopyToAsync(stream);
                }
                discussion.ImageFilename = fileName;
            }

            _context.Add(discussion);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        return View(discussion);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var discussion = await _context.Discussions.FindAsync(id);
        if (discussion == null) return NotFound();

        var userId = _userManager.GetUserId(User);
        if (discussion.ApplicationUserId != userId)
        {
            return Forbid();
        }

        return View(discussion);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFilename")] Discussion discussion, IFormFile? ImageFile)
    {
        if (id != discussion.DiscussionId) return NotFound();

        var existingDiscussion = await _context.Discussions.FindAsync(id);
        if (existingDiscussion == null) return NotFound();

        var userId = _userManager.GetUserId(User);
        if (existingDiscussion.ApplicationUserId != userId)
        {
            return Forbid();
        }

        if (ModelState.IsValid)
        {
            existingDiscussion.Title = discussion.Title;
            existingDiscussion.Content = discussion.Content;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                existingDiscussion.ImageFilename = fileName;
            }

            _context.Update(existingDiscussion);
            await _context.SaveChangesAsync();
            return RedirectToAction("DiscussionDetails", "Home", new { id = discussion.DiscussionId });
        }
        return View(discussion);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var discussion = await _context.Discussions.FindAsync(id);
        if (discussion == null) return NotFound();

        var userId = _userManager.GetUserId(User);
        if (discussion.ApplicationUserId != userId)
        {
            return Forbid();
        }

        return View(discussion);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var discussion = await _context.Discussions.FindAsync(id);
        if (discussion == null) return NotFound();

        var userId = _userManager.GetUserId(User);
        if (discussion.ApplicationUserId != userId)
        {
            return Forbid();
        }

        _context.Discussions.Remove(discussion);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }
}
