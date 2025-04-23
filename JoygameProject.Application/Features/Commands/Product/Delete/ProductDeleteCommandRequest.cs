using JoygameProject.Application.Wrappers;
using MediatR;

namespace JoygameProject.Application.Features.Commands.Product.Delete
{
    public class ProductDeleteCommandRequest : IRequest<ServiceResponse<bool>>
    {
        public int Id { get; set; }
    }
}
