using AssetLog.Api.DTOs;
using AssetLog.Api.Models;

namespace AssetLog.Api.Services.EquipmentServices
{
    public interface IEquipmentService
    {
        Task<List<EquipmentDto>> GetAllEquipment(CancellationToken cancellationToken = default);
        Task<EquipmentDto> GetEquipmentByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<EquipmentDto> CreateEquipment(CreateEquipmentDto newEquipment, CancellationToken cancellationToken = default);
        Task<EquipmentDto> UpdateEquipment(int id, UpdateEquipmentDto updatedEquipment, CancellationToken cancellationToken = default);
        Task<EquipmentDto> DeactivateEquipment(int id, CancellationToken cancellationToken = default);
        Task<EquipmentDto> ActivateEquipment(int id, CancellationToken cancellationToken = default);

    //adicionar 
    }
}