using Microsoft.AspNetCore.Mvc;

namespace AutofacTutorial.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }        
    }
}
