namespace JoygameProject.Domain.Entities
{
    public class RoleDetail : BaseEntity
    {
        public int PageId { get; set; }
        public int RoleId { get; set; }
        public virtual Page? Page { get; set; }
        public virtual Role? Role { get; set; }
        public bool CanView { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanInsert { get; set; }
        
    }
}
