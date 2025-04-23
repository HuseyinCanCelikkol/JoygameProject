using JoygameProject.Application.DTOs;
using JoygameProject.Application.Wrappers;
using MediatR;

namespace JoygameProject.Application.Features.Queries.Product.List
{
    public class ProductListQueryRequest : IRequest<ServiceResponse<List<ProductDto>>>
    {
    }
}
