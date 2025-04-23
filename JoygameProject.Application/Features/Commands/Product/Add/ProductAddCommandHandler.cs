using AutoMapper;
using JoygameProject.Application.Abstractions.Services;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace JoygameProject.Application.Features.Commands.Product.Add
{
    public class ProductAddCommandHandler(IMapper mapper, IUnitOfWork worker,ICacheService cache) : IRequestHandler<ProductAddCommandRequest, ServiceResponse<int>>
    {
        public async Task<ServiceResponse<int>> Handle(ProductAddCommandRequest request, CancellationToken cancellationToken)
        {
            ServiceResponse<int> res = new();
            Domain.Entities.Product product = mapper.Map<Domain.Entities.Product>(request);
            worker.Write<Domain.Entities.Product>().Add(product);
            await worker.SaveAsync();
            await cache.SetAsync($"product-detail-{product.Id}", product);
            await cache.RemoveAsync("product-list");

            return res.Success(product.Id);
        }
    }
}
