using JoygameProject.Application.Features.Queries.Category.List;
using JoygameProject.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace JoygameProject.Web.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class HomeController(ILogger<HomeController> logger, IMediator mediator) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await mediator.Send(new CategoryListQueryRequest());

            if (result.Status != HttpStatusCode.OK)
                return View("Error");

            return View(result.Result);
        }
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
