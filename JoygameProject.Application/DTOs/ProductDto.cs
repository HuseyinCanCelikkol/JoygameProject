namespace JoygameProject.Application.DTOs
{
    public class ProductDto
    {
        public required string Name { get; set; }     
        public string? Description { get; set; }
        public int CatId { get; set; }
        public required string ImageUrl { get; set; }
        public double Price { get; set; }
    };

}
