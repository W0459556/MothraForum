using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MothraForum.Models
{
    public class ApplicationUser : IdentityUser
    {
        public required string Name { get; set; }
        public string? Location { get; set; }

        public string ImageFilename { get; set; } = "default.jpg"; 

        [NotMapped]
        public IFormFile? ImageFile { get; set; } 
    }

}
