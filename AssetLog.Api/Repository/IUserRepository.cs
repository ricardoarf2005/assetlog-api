using AssetLog.Api.Models;

namespace AssetLog.Api.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user, CancellationToken cancellationToken = default);
        Task DeleteUser(int userId);
        Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default);
        Task<List<Equipment>> GetEquipmentsByUserId(int userId, CancellationToken cancellationToken = default);
        Task<User?> GetUserById(int userId, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}