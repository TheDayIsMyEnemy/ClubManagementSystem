using ClubManagementSystem.Data.Entities;

namespace ClubManagementSystem.Interfaces
{
    public interface IMemberService : IDisposable
    {
        Task<List<Member>> GetAllMembers();

        Task<bool> CreateMember(Member member);

        Task<bool> UpdateMember(Member member);

        Task<bool> DeleteMember(Member member);
    }
}
