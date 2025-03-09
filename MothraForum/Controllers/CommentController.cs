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
                return RedirectToAction("DiscussionDetails", "Home", new { id = discussionId });
            }

            var comment = new Comment
            {
                DiscussionId = discussionId,
                Content = content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("DiscussionDetails", "Home", new { id = discussionId });
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}