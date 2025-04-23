using AutoMapper;
using JoygameProject.Application.Abstractions.Services;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.DTOs;
using JoygameProject.Application.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoygameProject.Application.Features.Queries.Product.List
{
    internal class ProductListQueryHandler(IMapper mapper, IUnitOfWork worker,ICacheService cache) : IRequestHandler<ProductListQueryRequest, ServiceResponse<List<ProductDto>>>
    {
        public async Task<ServiceResponse<List<ProductDto>>> Handle(ProductListQueryRequest request, CancellationToken cancellationToken)
        {
            ServiceResponse<List<ProductDto>> res = new();
            var cached = await cache.GetAsync<List<ProductDto>>("product-list");
            if (cached is not null)
                return res.Success(cached);

            List<Domain.Entities.Product> products = await worker.Read<Domain.Entities.Product>().GetList(x => x.IsActive).ToListAsync(cancellationToken);

            await cache.SetAsync("product-list", mapper.Map<List<ProductDto>>(products));
            return res.Success(mapper.Map<List<ProductDto>>(products));
        }
    }
}
