namespace ClubManagementSystem.Data.Entities
{
    public class Group : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<Member> Members { get; set; } = null!;
    }
}
