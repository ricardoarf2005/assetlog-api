using AssetLog.Api.Models;

namespace AssetLog.Api.Repositories.EquipmentRepository
{
    public interface IEquipmentRepository
    {
        Task<List<Equipment>> GetAllEquipment(CancellationToken cancelationToken = default);
        Task<Equipment?> GetEquipmentById(int id, CancellationToken cancelationToken = default);
        Task AddEquipment(Equipment equipment, CancellationToken cancelationToken = default);
        Task SaveChangesAsync(CancellationToken cancelationToken = default);
    }
}