using AssetLog.Api.DTOs;
using AssetLog.Api.Models;
using AssetLog.Api.Models.Enums;
using AssetLog.Api.Repositories.UserRepository;
using Microsoft.IdentityModel.Logging;

namespace AssetLog.Api.Services.UserServices
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<List<UserDto>> GetAllUsers(CancellationToken cancellationToken = default)
        {
            var users = await userRepository.GetAllUsers(cancellationToken);
            return users.Select(MapToUserDto).ToList();
        }


        public async Task<UserDto> GetUsersById(int userId, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetUserById(userId,cancellationToken)
                ?? throw new KeyNotFoundException($"User with ID {userId} was not found.");
            return MapToUserDto(user);
        }

        public async Task<List<EquipmentDto>> GetEquipmentByUserId(int userId, CancellationToken cancellationToken = default)
        {
            var users = await userRepository.GetUserById(userId, cancellationToken)
            ?? throw new KeyNotFoundException($"User with ID {userId} was not found.");

            //var equipment = 

            return new List<EquipmentDto>();

        }

        private static UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Department = user.Department,
                IsActive = user.IsActive,
                EndDate = user.EndDate,
                TypeOfContract = user.TypeOfContract
            };
        }
    }
}