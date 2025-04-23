namespace JoygameProject.Application.DTOs
{
    public class CategoryTreeDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<ProductMiniDto> Products { get; set; } = new();
        public List<CategoryTreeDto>? Children { get; set; } = [];
    }
}
