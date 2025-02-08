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
    public class CommentController : Controller
    {
        private readonly MothraForumContext _context;

        public CommentController(MothraForumContext context)
        {
            _context = context;
        }

        // GET: Comment/Create/5
        public IActionResult Create(int discussionId)
        {
            ViewData["DiscussionId"] = discussionId;
            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        
        public async Task<IActionResult> AddComment(int discussionId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return RedirectToAction("Details", "Discussion", new { id = discussionId });
            }

            var comment = new Comment
            {
                DiscussionId = discussionId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Discussion", new { id = discussionId });
        }


        // GET: Comment/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comment/Edit/5
        [HttpPost]
        
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,Content")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException){
                    if (!CommentExists(comment.CommentId)){
                        return NotFound();
                    }else{
                        throw;
                    }
                }
                return RedirectToAction("Details", "Discussion", new { id = comment.DiscussionId });
            }
            return View(comment);
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Discussion", new { id = comment.DiscussionId });
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}