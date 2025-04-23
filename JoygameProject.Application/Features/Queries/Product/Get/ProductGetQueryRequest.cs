using JoygameProject.Application.DTOs;
using JoygameProject.Application.Wrappers;
using MediatR;

namespace JoygameProject.Application.Features.Queries.Product.Get
{
    public class ProductGetQueryRequest : IRequest<ServiceResponse<ProductDto>>
    {
        public int Id { get; set; }
    }
}
