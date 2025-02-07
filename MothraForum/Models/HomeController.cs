using Microsoft.AspNetCore.Mvc;

namespace MothraForum.Models
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
            // i created this accidentally and we are pretending we do not see it. agreed? agreed. nice disciussion. 
        }
    }
}
