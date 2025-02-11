using Microsoft.AspNetCore.Mvc;

namespace EmailMarketing.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return PhysicalFile("wwwroot/html/index.html", "text/html");
        }
    }
}
