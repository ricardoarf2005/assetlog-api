using AssetLog.Api.DTOs;
using AssetLog.Api.Models.Enums;
using AssetLog.Api.Services.EquipmentServices;
using Microsoft.AspNetCore.Mvc;


namespace AssetLog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController(IEquipmentService equipmentService) : ControllerBase
    {
        //POST/api/equipment -> adicionar material
        [HttpPost()]
        public async Task<ActionResult<EquipmentDto>> CreateEquipment(
            [FromBody] CreateEquipmentDto newEquipment,
            CancellationToken cancellationToken)
        {
            try
            {
                var equipment = await equipmentService.CreateEquipment(newEquipment, cancellationToken);
                return CreatedAtAction(nameof(GetEquipmentById), new { id = equipment.Id }, equipment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //GET/api/equipment/{id} -> mostrar equipamento por id
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentDto>> GetEquipmentById(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var equipment = await equipmentService.GetEquipmentByIdAsync(id, cancellationToken);
            if (equipment == null)
            {
                return NotFound();
            }
            return Ok(equipment);
        }

    

        //PUT/api/equipment/{id} -> atualizar parametro do equipamento
        [HttpPut("{id}")]
        public async Task<ActionResult<EquipmentDto>> UpdateEquipment(
            int id,
            [FromBody] UpdateEquipmentDto updateDto,
            CancellationToken cancellationToken)
        {
            var equipment = await equipmentService.UpdateEquipment(id, updateDto, cancellationToken);

            return Ok(equipment);
        }

        //GET/api/equipment -> mostrar todos os equipamentos (permitir filtar por categoria e disponilidade eventualmente)
        [HttpGet()]
        public async Task<ActionResult<List<EquipmentDto>>> GetEquipment(
            CancellationToken cancellationToken)
        {
            var equipments = await equipmentService.GetAllEquipment(cancellationToken);
            return Ok(equipments);
        }

        //PUT/api/equipment/{id}/deactivate -> atualizar isActive para false
        [HttpPut("{id}/deactivate")]
        public async Task<ActionResult<EquipmentDto>> DeactivateEquipment(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var equipment = await equipmentService.DeactivateEquipment(id, cancellationToken);
            return CreatedAtAction(nameof(GetEquipmentById), new { id = equipment.Id }, equipment);
        }

        //PUT/api/equipment/{id}/activate -> atualizar isActive para true
        [HttpPut("{id}/activate")]
        public async Task<ActionResult<EquipmentDto>> ActivateEquipment(
            [FromRoute] int id,
            CancellationToken cancellationToken)
        {
            var equipment = await equipmentService.ActivateEquipment(id, cancellationToken);
            return CreatedAtAction(nameof(GetEquipmentById), new { id = equipment.Id }, equipment);
        }
    }
}

