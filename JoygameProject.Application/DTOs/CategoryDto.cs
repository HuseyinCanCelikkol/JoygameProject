using JoygameProject.Domain.Entities;

namespace JoygameProject.Application.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public virtual List<CategoryDto>? InverseParent { get; set; }
        public bool IsActive { get; set; }
    }
}
