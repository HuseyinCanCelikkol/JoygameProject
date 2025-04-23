using JoygameProject.Application.DTOs;
using JoygameProject.Application.Wrappers;
using MediatR;

namespace JoygameProject.Application.Features.Commands.Product.Update
{
    public class ProductUpdateCommandRequest : ProductDto,IRequest<ServiceResponse<ProductDto>>
    {
        public int Id { get; set; }
    }
}
