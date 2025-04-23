using JoygameProject.Application.Features.Queries.Category.List;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JoygameProject.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCategoryTree()
        {
            var result = await mediator.Send(new CategoryListQueryRequest());
            return Ok(result);
        }
    }
}
