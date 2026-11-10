using AssetLog.Api.DTOs;
using AssetLog.Api.Models.Enums;
using AssetLog.Api.Models;
using AssetLog.Api.Repositories.EquipmentRepository;



namespace AssetLog.Api.Services.EquipmentServices
{
    public class EquipmentService(IEquipmentRepository equipmentRepository) : IEquipmentService
    {
        public async Task<EquipmentDto> CreateEquipment(CreateEquipmentDto newEquipment, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(newEquipment.Name))
            {
                throw new ArgumentException("Equipment name cannot be empty.");
            }
            if (newEquipment.Stock < newEquipment.MinStock)
            {
                throw new ArgumentException("Can't create equipment with stock lower than its minimum stock value.");
            }
        
            var equip = new Equipment
            {
                Name = newEquipment.Name,
                Description = newEquipment.Description,
                Category = newEquipment.Category,
                MinStock = newEquipment.MinStock,
                Stock = newEquipment.Stock,
                IsActive = true
            };
            await equipmentRepository.AddEquipment(equip, cancellationToken);
            await equipmentRepository.SaveChangesAsync(cancellationToken);

            return MapToEquipmentDto(equip!);
        }

        public async Task<EquipmentDto> UpdateEquipment(int Id, UpdateEquipmentDto updatedEquipment, CancellationToken cancellationToken = default)
        {
            var equipment = await equipmentRepository.GetEquipmentById(Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Equipment with ID {Id} not found.");

            if (equipment == null)
            {
                throw new ArgumentException("Equipment not found.");
            }
            
            if (string.IsNullOrWhiteSpace(updatedEquipment.Name))
            {
                throw new ArgumentException("Equipment name cannot be empty.");
            }

            if (updatedEquipment.Name != null)
            {
                equipment.Name = updatedEquipment.Name;
            }
            if (updatedEquipment.Description != null)
            {
                equipment.Description = updatedEquipment.Description;
            }
            if (updatedEquipment.Category != null)
            {
                equipment.Category = updatedEquipment.Category.Value;
            }
            if (updatedEquipment.MinStock.HasValue)
            {
                equipment.MinStock = updatedEquipment.MinStock.Value;
            }
            if (updatedEquipment.Stock.HasValue)
            {
                equipment.Stock = updatedEquipment.Stock.Value;
            }

            await equipmentRepository.SaveChangesAsync(cancellationToken);

            return MapToEquipmentDto(equipment);
        }
        public async Task<EquipmentDto> GetEquipmentByIdAsync(int Id, CancellationToken cancellationToken = default)
        {
            var equipment = await equipmentRepository.GetEquipmentById(Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Equipment with ID {Id} not found.");
            return MapToEquipmentDto(equipment);
        }
        public async Task<List<EquipmentDto>> GetAllEquipment(CancellationToken cancellationToken = default)
        {
            var equipments = await equipmentRepository.GetAllEquipment(cancellationToken);
            return equipments.Select(MapToEquipmentDto).ToList();
        }

        public async Task<EquipmentDto> DeactivateEquipment(int Id, CancellationToken cancellationToken = default)
        {
            var equipment = await equipmentRepository.GetEquipmentById(Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Equipment with ID {Id} not found.");

            equipment.IsActive = false;
            await equipmentRepository.SaveChangesAsync(cancellationToken);

            return MapToEquipmentDto(equipment);
        }

        public async Task<EquipmentDto> ActivateEquipment(int Id, CancellationToken cancellationToken = default)
        {
            var equipment = await equipmentRepository.GetEquipmentById(Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Equipment with ID {Id} not found.");

            equipment.IsActive = true;
            await equipmentRepository.SaveChangesAsync(cancellationToken);

            return MapToEquipmentDto(equipment);
        }


        private static EquipmentDto MapToEquipmentDto(Equipment equipment)
        {
            return new EquipmentDto
            {
                Id = equipment.Id,
                Name = equipment.Name,
                Description = equipment.Description,
                Category = equipment.Category,
                IsActive = equipment.IsActive,
                MinStock = equipment.MinStock,
                Stock = equipment.Stock
            };
        }
    }


}