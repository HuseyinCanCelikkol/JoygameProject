using AutoMapper;
using JoygameProject.Application.Abstractions.Services;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.DTOs;
using JoygameProject.Application.Wrappers;
using MediatR;

namespace JoygameProject.Application.Features.Commands.Product.Update
{
    internal class ProductUpdateCommandHandler(IUnitOfWork worker, IMapper mapper, ICacheService cache) : IRequestHandler<ProductUpdateCommandRequest, ServiceResponse<ProductDto>>
    {
        public async Task<ServiceResponse<ProductDto>> Handle(ProductUpdateCommandRequest request, CancellationToken cancellationToken)
        {
            ServiceResponse<ProductDto> res = new();

            Domain.Entities.Product? foundProduct = await worker.Read<Domain.Entities.Product>().GetByIdAsync(request.Id);
            if (foundProduct == null)
            {
                return res.NotFound();
            }

            foundProduct.Name = request.Name;
            foundProduct.Description = request.Description;
            foundProduct.CatId = request.CatId;
            foundProduct.ImageUrl = request.ImageUrl;
            foundProduct.Price = request.Price;

            worker.Write<Domain.Entities.Product>().Update(foundProduct);
            await worker.SaveAsync();
            await cache.RemoveAsync($"product-detail-{request.Id}");
            await cache.RemoveAsync("product-list");
            return res.Success(mapper.Map<ProductDto>(foundProduct));

        }
    }

}
