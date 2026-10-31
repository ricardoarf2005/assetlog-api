using AssetLog.Api.Data;
using AssetLog.Api.Models;
using AssetLog.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using System.Diagnostics;

namespace AssetLog.Api.Repositories.UserRepository
{
    public class UserRepository(AppDbContext db) : IUserRepository
    {
        public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default)
        {
            return await db.Users.ToListAsync(cancellationToken);
        }

        public async Task<User?> GetUserById(int userId, CancellationToken cancellationToken = default)
        {
            return await db.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public async Task<User> AddUser(User user, CancellationToken cancellationToken = default)
        {
            var users = await db.Users.AddAsync(user, cancellationToken);
            await SaveChangesAsync();
            return users.Entity;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUser(int userId)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user != null)
            {
                db.Users.Remove(user);
                await SaveChangesAsync();
            }
        }

        public async Task<List<Equipment>> GetEquipmentsByUserId(int userId, CancellationToken cancellationToken = default)
        {
            var equipment = db.Orders
                                .Include(o => o.OrderItems)
                                    .ThenInclude(o => o.Equipment)
                                .Where(o => o.UserId == userId && (o.Status == StatusEnum.APPROVED || o.Status == StatusEnum.ONGOING));

            var output = new List<Equipment>();
            return output;

        }
    }
}
