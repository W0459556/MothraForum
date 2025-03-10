using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MothraForum.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; } // uid

        [Required]
        [StringLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [NotMapped]
        [Display(Name = "Attach Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ImageFilename { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Comment>? Comments { get; set; }
        public List<Vote>? Votes { get; set; }

        public string? ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser? User { get; set; }
    }
}
