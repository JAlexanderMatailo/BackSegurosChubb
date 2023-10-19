using Microsoft.AspNetCore.Mvc;

namespace BackSegurosChubb.Controllers
{
    public class SeguroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
