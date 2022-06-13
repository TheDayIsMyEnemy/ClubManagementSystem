using ClubManagementSystem.Data.Entities;

namespace ClubManagementSystem.Interfaces
{
    public interface IMembershipService
    {
        Task<List<Membership>> GetAllMemberships();
    }
}
