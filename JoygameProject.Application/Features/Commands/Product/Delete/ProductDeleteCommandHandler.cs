using JoygameProject.Application.Abstractions.Services;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.Wrappers;
using MediatR;

namespace JoygameProject.Application.Features.Commands.Product.Delete
{
    public class ProductDeleteCommandHandler(IUnitOfWork worker, ICacheService cache) : IRequestHandler<ProductDeleteCommandRequest, ServiceResponse<bool>>
    {
        public async Task<ServiceResponse<bool>> Handle(ProductDeleteCommandRequest request, CancellationToken cancellationToken)
        {
            ServiceResponse<bool> res = new();
            bool isDeleted = worker.Write<Domain.Entities.Product>().RemoveById(request.Id);
            if(!isDeleted)
            {
                return res.NotFound();
            }

            await worker.SaveAsync();
            await cache.RemoveAsync($"product-detail-{request.Id}");
            await cache.RemoveAsync("product-list");
            return res.Success(isDeleted);
        }
    }
}
