using JoygameProject.Application.Abstractions.Services;
using JoygameProject.Application.Abstractions.UnitOfWorks;
using JoygameProject.Application.DTOs;
using JoygameProject.Application.Wrappers;
using MediatR;

namespace JoygameProject.Application.Features.Queries.Category.List
{
    public class CategoryListQueryHandler(IUnitOfWork worker, ICacheService cache) : IRequestHandler<CategoryListQueryRequest, ServiceResponse<List<CategoryTreeDto>>>
    {
        public async Task<ServiceResponse<List<CategoryTreeDto>>> Handle(CategoryListQueryRequest request, CancellationToken cancellationToken)
        {
            ServiceResponse<List<CategoryTreeDto>> res = new();
            var cached = await cache.GetAsync<List<CategoryTreeDto>>("category-list");
            if (cached is not null)
                return res.Success(cached);

            var flatList = await worker.ExecuteSqlAsync(
                "sp_GetCategoryHierarchy",
                reader => new FlatCategoryDto
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? null : reader.GetInt32(reader.GetOrdinal("ParentId")),
                    ProductId = reader.IsDBNull(reader.GetOrdinal("ProductId")) ? null : reader.GetInt32(reader.GetOrdinal("ProductId")),
                    ProductName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? null : reader.GetString(reader.GetOrdinal("ProductName")),
                    ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                    Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? null : (float?)reader.GetDouble(reader.GetOrdinal("Price"))
                });

            var lookup = flatList.ToLookup(x => x.ParentId);

            List<CategoryTreeDto> BuildTree(int? parentId)
            {
                return lookup[parentId]
                    .GroupBy(x => x.Id)
                    .Select(group =>
                    {
                        CategoryTreeDto category = new()
                        {
                            Id = group.Key,
                            Name = group.First().Name,
                            Products = group
                                .Where(x => x.ProductId.HasValue)
                                .Select(p => new ProductMiniDto
                                {
                                    Id = p.ProductId!.Value,
                                    Name = p.ProductName!,
                                    ImageUrl = p.ImageUrl!,
                                    Price = p.Price!.Value
                                }).ToList(),
                            Children = BuildTree(group.Key)
                        };

                        // childdeki ürünleri de parente ver
                        foreach (var child in category.Children)
                        {
                            category.Products.AddRange(child.Products);
                        }

                        return category;
                    })
                    .ToList();
            }

            return res.Success(BuildTree(null));
        }
    }
}
