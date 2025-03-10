using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MothraForum.Models
{
    public class Vote
    {
        public int VoteId { get; set; } // uid

        public int DiscussionId { get; set; } // discussion being voted on

        public Discussion Discussion { get; set; } = null!;

        public string ApplicationUserId { get; set; } = null!; // user who voted

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser User { get; set; } = null!; 

        public int Value { get; set; } // +1 for upvote, -1 for downvote
    }
}
