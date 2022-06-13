using ClubManagementSystem.Enums;

namespace ClubManagementSystem.Interfaces
{
    public interface IUploadFileService
    {
        Task<(UploadMembersOutcome, int?)> UploadMembers(Stream csvStream);
    }
}
