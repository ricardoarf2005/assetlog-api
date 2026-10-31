using AssetLog.Api.DTOs;

namespace AssetLog.Api.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsers(CancellationToken cancellationToken = default);
        Task<List<EquipmentDto>> GetEquipmentByUserId(int userId, CancellationToken cancellationToken = default);
        Task<UserDto> GetUsersById(int userId, CancellationToken cancellationToken = default);
    }
}