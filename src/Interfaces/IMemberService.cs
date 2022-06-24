using ClubManagementSystem.Data.Entities;

namespace ClubManagementSystem.Interfaces
{
    public interface IMemberService : IDisposable
    {
        Task<List<Member>> GetAllMembers();

        Task<bool> CreateMember(Member member);

        Task<bool> UpdateMember(Member member);

        Task<bool> RenewMembership(Member member, Membership newMembership);

        Task<bool> DeleteMember(Member member);
    }
}
