using AutoMapper;
using JoygameProject.Application.Abstractions.Services;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.DTOs;
using JoygameProject.Application.Wrappers;
using MediatR;
using System.Xml.Linq;

namespace JoygameProject.Application.Features.Queries.Product.Get
{
    public class ProductGetQueryHandler(IMapper mapper, IUnitOfWork worker, ICacheService cache) : IRequestHandler<ProductGetQueryRequest, ServiceResponse<ProductDto>>
    {
        public async Task<ServiceResponse<ProductDto>> Handle(ProductGetQueryRequest request, CancellationToken cancellationToken)
        {
            ServiceResponse<ProductDto> res = new();
            var cached = await cache.GetAsync<ProductDto>($"product-detail-{request.Id}");
            if (cached is not null)
                return res.Success(cached);
            

            Domain.Entities.Product? foundProduct = await worker.Read<Domain.Entities.Product>().GetByIdAsync(request.Id);
            if(foundProduct == null)
            {
                return res.NotFound();
            }
            await cache.SetAsync($"product-detail-{request.Id}", foundProduct);
            return res.Success(mapper.Map<ProductDto>(foundProduct));  
        }
    }
}
