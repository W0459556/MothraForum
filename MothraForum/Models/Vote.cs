using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using MothraForum.Models;


namespace MothraForum.Models
{
    public class Vote
    {
        public int VoteId { get; set; } // uid

        public int DiscussionId { get; set; } // the discussion

        [Range(-1, 1)] // +1 for upvote -1 for downvote
        public int Value { get; set; }

        public Discussion? Discussion { get; set; } // nav prop
    }
}
