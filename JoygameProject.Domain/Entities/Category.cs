namespace JoygameProject.Domain.Entities
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }

        // Parent (üst kategori)
        public int? ParentId { get; set; }
        public virtual Category? Parent { get; set; }

        // Children (alt kategoriler)
        public virtual ICollection<Category>? InverseParent { get; set; }
    }
}
