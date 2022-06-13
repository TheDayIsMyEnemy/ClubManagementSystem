namespace ClubManagementSystem.Data.Entities
{
    public class Membership : AuditableEntity
    {     
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Fee { get; set; }

        public int MemberId { get; set; }

        public Member Member { get; set; } = null!;

        public bool IsActive => DateTime.Now.Date <= EndDate.Date;
    }
}
