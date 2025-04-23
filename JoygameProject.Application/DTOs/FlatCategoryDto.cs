namespace JoygameProject.Application.DTOs
{
    public class FlatCategoryDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int? ParentId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ImageUrl { get; set; }
        public float? Price { get; set; }
    }
}
