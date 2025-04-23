using JoygameProject.Application.Features.Queries.Product.Get;
using JoygameProject.Web.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JoygameProject.Web.Controllers
{
    [Route("{slug}/{id}")]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [PermissionAuthorize("product", "canview")]
        [HttpGet]
        public async Task<IActionResult> Details(string slug, int id)
        {
            var result = await _mediator.Send(new ProductGetQueryRequest { Id = id });

            if (result.Status != HttpStatusCode.OK || result.Result == null)
                return NotFound();

            return View(result.Result);
        }
    }
}
