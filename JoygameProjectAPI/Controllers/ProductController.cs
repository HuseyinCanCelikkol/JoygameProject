using JoygameProject.API.Attributes;
using JoygameProject.Application.DTOs;
using JoygameProject.Application.Features.Commands.Product.Add;
using JoygameProject.Application.Features.Commands.Product.Delete;
using JoygameProject.Application.Features.Commands.Product.Update;
using JoygameProject.Application.Features.Queries.Product.Get;
using JoygameProject.Application.Features.Queries.Product.List;
using JoygameProject.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JoygameProject.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        [PermissionAuthorize("product", "canview")]
        [HttpGet("[action]")]
        public async Task<ServiceResponse<List<ProductDto>>> GetAllAsync()
        {
            return await mediator.Send(new ProductListQueryRequest());
        }

        [PermissionAuthorize("product", "canview")]
        [HttpGet("{id:int}")]
        public async Task<ServiceResponse<ProductDto>> GetByIdAsync(int id)
        {
            return await mediator.Send(new ProductGetQueryRequest() { Id = id});
        }

        [PermissionAuthorize("product", "caninsert")]
        [HttpPost("[action]")]
        public async Task<ServiceResponse<int>> AddAsync([FromBody] ProductAddCommandRequest dto)
        {
            return await mediator.Send(dto);
        }

        [PermissionAuthorize("product", "canupdate")]
        [HttpPut("[action]")]
        public async Task<ServiceResponse<ProductDto>> UpdateAsync([FromBody] ProductUpdateCommandRequest dto)
        {
            return await mediator.Send(dto);
        }

        [PermissionAuthorize("product", "candelete")]
        [HttpDelete("{id:int}")]
        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            return await mediator.Send(new ProductDeleteCommandRequest() { Id = id});
        }
    }
}
