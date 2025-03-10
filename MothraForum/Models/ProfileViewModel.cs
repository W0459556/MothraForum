namespace MothraForum.Models
{
    public class ProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Discussion> Discussions { get; set; }
    }
}
