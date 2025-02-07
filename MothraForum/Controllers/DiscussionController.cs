using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MothraForum.Data;
using MothraForum.Models;

namespace MothraForum.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly MothraForumContext _context;

        public DiscussionController(MothraForumContext context)
        {
            _context = context;
        }

        // get the discussion
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussions
                .Include(d => d.Comments)
                .Include(d => d.Votes)
                .ToListAsync();
            return View(discussions);
        }

        // get the votes & comments of the discussion: 
        public async Task<IActionResult> Details(int? id){
            if (id == null){
                return NotFound();
            }

            var discussion = await _context.Discussions
                .Include(d => d.Comments)
                .Include(d => d.Votes)
                .FirstOrDefaultAsync(d => d.DiscussionId == id);

            if (discussion == null){
                return NotFound();
            }

            return View(discussion);
        }

        // create discussion view
        public IActionResult Create()
        {
            return View();
        }

        // create discussion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,ImageFile")] Discussion discussion){
            if (ModelState.IsValid){
                discussion.CreatedAt = DateTime.Now;

                // image upload
                if (discussion.ImageFile != null){
                    var fileName = Path.GetFileName(discussion.ImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create)){
                        await discussion.ImageFile.CopyToAsync(stream);
                    }
                    discussion.ImageFilename = fileName;
                }

                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }

        // get discussion edit
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

        // post discussion edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,ImageFilename")] Discussion discussion)
        {
            if (id != discussion.DiscussionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscussionExists(discussion.DiscussionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }

        // get delete
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

        // post delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussions.FindAsync(id);
            _context.Discussions.Remove(discussion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // post to vote
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                _context.Add(vote); // add the vote
                // u can vote infinity times right now because we;re doing user stuff later. ill come back and change this then 

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        private bool DiscussionExists(int id)
        {
            return _context.Discussions.Any(e => e.DiscussionId == id);
        }
    }
}
