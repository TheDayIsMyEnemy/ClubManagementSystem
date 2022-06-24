namespace ClubManagementSystem.Data.Entities
{
    public class Member : AuditableEntity
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string FullName => $"{FirstName} {LastName}";

        public Gender Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? PhoneNumber { get; set; }

        public Membership? Membership { get; set; }

        public ICollection<Group> Groups { get; set; } = null!;

        public ICollection<Transaction> Transactions { get; set; } = null!;

        public ICollection<MembershipHistory> MembershipHistory { get; set; } = null!;
    }
}
