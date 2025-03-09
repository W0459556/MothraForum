using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MothraForum.Data;
using MothraForum.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

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
                                       .Include(d => d.Comments)
                                       .Include(d => d.Votes)
                                       .OrderByDescending(d => d.CreatedAt)
                                       .FirstOrDefaultAsync(d => d.DiscussionId == id);

        if (discussion == null)
        {
            return NotFound();
        }

        return View(discussion);
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
            return RedirectToAction("Index");
        }
        return View(discussion);
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var discussion = await _context.Discussions.FindAsync(id);
        if (discussion == null)
        {
            return NotFound();
        }
        return View(discussion);
    }


    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFilename")] Discussion discussion, IFormFile? ImageFile)
    {
        if (id != discussion.DiscussionId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var existingDiscussion = await _context.Discussions.FindAsync(id);
                if (existingDiscussion == null)
                {
                    return NotFound();
                }

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
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Discussions.Any(e => e.DiscussionId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("DiscussionDetails", new { id = discussion.DiscussionId });
        }
        return View(discussion);
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var discussion = await _context.Discussions
            .FirstOrDefaultAsync(d => d.DiscussionId == id);
        if (discussion == null)
        {
            return NotFound();
        }

        return View(discussion);
    }


    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var discussion = await _context.Discussions.FindAsync(id);
        _context.Discussions.Remove(discussion);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
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

    private bool DiscussionExists(int id)
    {
        return _context.Discussions.Any(e => e.DiscussionId == id);
    }
}
