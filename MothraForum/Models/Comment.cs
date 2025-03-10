using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MothraForum.Models
{
    public class Comment
    {
        public int CommentId { get; set; } // uid 

        [Required]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int DiscussionId { get; set; } // the discussion the comment belongs to

        public Discussion? Discussion { get; set; } // nav property

        public string? ApplicationUserId { get; set; } 

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? User { get; set; } 
    }
}
