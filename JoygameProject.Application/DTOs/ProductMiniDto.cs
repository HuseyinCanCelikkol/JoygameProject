namespace JoygameProject.Application.DTOs
{
    public class ProductMiniDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public float Price { get; set; }

    }
}
