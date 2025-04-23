namespace JoygameProject.Domain.Entities
{
    public class Role : BaseEntity
    {
        public required string Name { get; set; }
        public virtual ICollection<RoleDetail>? RoleDetails {  get; set; }  

    }
}
