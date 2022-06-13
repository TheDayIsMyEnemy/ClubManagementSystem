namespace ClubManagementSystem.Enums
{
    public enum UploadMembersOutcome
    {
        Success,
        FileNotFound,
        EmptyFile,
        MissingRequiredColumns,
        InvalidFile,
        InsertFailed
    }
}
