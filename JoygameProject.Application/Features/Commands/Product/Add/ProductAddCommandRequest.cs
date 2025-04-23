using JoygameProject.Application.DTOs;
using JoygameProject.Application.Wrappers;
using MediatR;

namespace JoygameProject.Application.Features.Commands.Product.Add
{
    public class ProductAddCommandRequest : ProductDto, IRequest<ServiceResponse<int>>
    {
    }
}
