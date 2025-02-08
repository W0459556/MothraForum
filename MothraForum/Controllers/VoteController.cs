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
    public class VoteController : Controller
    {
        private readonly MothraForumContext _context;

        public VoteController(MothraForumContext context)
        {
            _context = context;
        }

        // POST: Vote/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int discussionId, int value)
        {
            var vote = new Vote
            {
                DiscussionId = discussionId,
                Value = value
            };

            _context.Add(vote);
            await _context.SaveChangesAsync(); // add vote & save

            return RedirectToAction("Index", "Home", new { id = discussionId }); // Redirect to the discussion details page
        }
    }
}