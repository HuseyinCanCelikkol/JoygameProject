using System.ComponentModel.DataAnnotations.Schema;

namespace JoygameProject.Domain.Entities
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        [ForeignKey("Category")]
        public int CatId { get; set; }
        public virtual Category? Category { get; set; }
        public required string ImageUrl { get; set; }
        public double Price { get; set; }
        public string?  Description { get; set; }
    }
}
