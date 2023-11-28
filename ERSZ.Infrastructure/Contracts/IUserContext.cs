namespace ERSZ.Infrastructure.Contracts
{
    public interface IUserContext
    {
        string UserId { get; }
        string Email { get; }
        string LogName { get; }
        string FullName { get; }
        int? CourtId { get; }
        string CourtName { get; }

        bool IsUserInRole(string role);
    }
}
