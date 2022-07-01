using Microsoft.AspNetCore.Mvc;

namespace SingleStoneScreen.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Index.cshtml");
        }
    }
}
