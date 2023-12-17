namespace Shipfinity.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CountByRole(string role);
    }
}
