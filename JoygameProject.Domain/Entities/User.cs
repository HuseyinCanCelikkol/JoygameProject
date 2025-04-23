namespace JoygameProject.Domain.Entities
{
    public class User : BaseEntity
    {
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
