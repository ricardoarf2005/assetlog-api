using AssetLog.Api.DTOs;
using AssetLog.Api.Models;
using AssetLog.Api.Repositories.UserRepository;

namespace AssetLog.Api.Services.UserService
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
            var user = await userRepository.GetUserById(userId, cancellationToken)
                ?? throw new KeyNotFoundException($"User with ID {userId} was not found.");
            return MapToUserDto(user);
        }

        public async Task<UserDto> CreateUser(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var existingUser = await userRepository.GetUserByEmail(dto.Email, cancellationToken);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already registered.");
            }
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                Department = dto.Department,
                EndDate = dto.EndDate??DateTime.MaxValue,
                IsActive = true,
                TypeOfContract = dto.TypeOfContract
            };
            
            var createdUser = await userRepository.AddUser(user, cancellationToken);
            return MapToUserDto(createdUser);
        }

        public async Task<UserDto> UpdateUser(int userId, UserDto dto, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetUserById(userId, cancellationToken)
                ?? throw new KeyNotFoundException($"User with ID {userId} was not found.");
            user.Name = dto.Name;
            user.Role = dto.Role;
            user.Department = dto.Department;
            user.IsActive = dto.IsActive;
            user.EndDate = dto.EndDate ?? DateTime.MaxValue;
            user.TypeOfContract = dto.TypeOfContract;

            await userRepository.SaveChangesAsync(cancellationToken);
            return MapToUserDto(user);
        }


        public async Task DeleteUser(int userId, CancellationToken cancellationToken = default)
        {
            var user = await userRepository.GetUserById(userId, cancellationToken)
                ?? throw new KeyNotFoundException($"User with ID {userId} was not found.");
            await userRepository.DeleteUser(userId);
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