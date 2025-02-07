using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MothraForum.Models;


/*
 * so, im more used to doing stuff with mongo schemas
 * and they kinda have built-in validation thingies. im so good at explaining
 * anyways, after a bit of digging i found:
 * https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/mvc-music-store/mvc-music-store-part-6
 * which is why i have:
 * using System.ComponentModel.DataAnnotations;, and 
 * using System.ComponentModel.DataAnnotations.Schema;
 * so that i can have a bit of data validation, make fields required, all that.
 * i really wanna get 100. please give me 100. please please please please please please please please please please please please please please please please
 */
namespace MothraForum.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; } // uid

        [Required] // make it a required field
        [StringLength(255)] // give it a max length. the sky is blue. other obvious statements
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [NotMapped]
        [Display(Name = "Attach Image")]

        public IFormFile? ImageFile { get; set; } 

        public string? ImageFilename { get; set; }

        [Display(Name = "Date Created")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public List<Comment>? Comments { get; set; } // 1 discus X comm
        public List<Vote>? Votes { get; set; } // 1 discus X up/downvote. X means many. in my brain. 
    }
}
