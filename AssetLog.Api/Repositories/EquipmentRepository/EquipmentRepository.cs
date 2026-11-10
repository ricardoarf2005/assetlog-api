using AssetLog.Api.Data;
using AssetLog.Api.Models;
using AssetLog.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace AssetLog.Api.Repositories.EquipmentRepository
{
    public class EquipmentRepository(AppDbContext db) : IEquipmentRepository
    {
        public async Task<List<Equipment>> GetAllEquipment(CancellationToken cancellationToken = default)
        {
            return await db.Equipments.ToListAsync(cancellationToken);
        }

        public async Task<Equipment?> GetEquipmentById(int id, CancellationToken cancelationToken = default)
        {
            return await db.Equipments.FindAsync([id], cancelationToken);
        }

        public async Task<List<Equipment>> GetEquipmentsByIds(IEnumerable<int> equipmentIds, CancellationToken cancellationToken = default)
        {
            var ids = equipmentIds.Distinct().ToList();
            return await db.Equipments
                .Where(e => ids.Contains(e.Id))
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Equipment>> GetEquipmentsByUserId(int userId, CancellationToken cancellationToken = default)
        {
            var equipment = db.Orders
                                .Include(o => o.OrderItems)
                                    .ThenInclude(o => o.Equipment)
                                .Where(o => o.UserId == userId && (o.Status == StatusEnum.ONGOING))
                                .SelectMany(x => x.OrderItems.Select(e => e.Equipment));

            return equipment.ToList();
        }

        public async Task AddEquipment(Equipment equipment, CancellationToken cancelationToken = default)
        {
            await db.Equipments.AddAsync(equipment, cancelationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancelationToken = default)
        {
            await db.SaveChangesAsync(cancelationToken);
        }
    }
    
}