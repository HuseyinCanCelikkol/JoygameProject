using Microsoft.AspNetCore.Mvc;

namespace JoygameProject.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Forbidden")]
        public IActionResult Forbidden()
        {
            return View();
        }
    }
}
