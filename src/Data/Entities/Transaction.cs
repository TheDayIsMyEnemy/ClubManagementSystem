namespace ClubManagementSystem.Data.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public double Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public TransactionType TransactionType { get; set; }

        public int? MemberId { get; set; }
    }
}
