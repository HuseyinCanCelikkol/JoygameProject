using JoygameProject.Application.DTOs;
using JoygameProject.Application.Wrappers;
using MediatR;

namespace JoygameProject.Application.Features.Queries.Category.List
{
    public class CategoryListQueryRequest : IRequest<ServiceResponse<List<CategoryTreeDto>>>
    {
    }
}
