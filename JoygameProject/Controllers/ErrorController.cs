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

        [Route("Unauthorized")]
        public IActionResult Unauthorized()
        {
            return View();
        }

        [Route("NotFound")]
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
