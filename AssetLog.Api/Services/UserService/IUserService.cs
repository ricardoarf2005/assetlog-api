using AssetLog.Api.DTOs;

namespace AssetLog.Api.Services.UserService
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(CreateUserDto dto, CancellationToken cancellationToken = default);
        Task DeleteUser(int userId, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetAllUsers(CancellationToken cancellationToken = default);
        Task<UserDto> GetUsersById(int userId, CancellationToken cancellationToken = default);
        Task<UserDto> UpdateUser(int userId, UserDto dto, CancellationToken cancellationToken = default);
    }
}