using AssetLog.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace AssetLog.Api.DTOs
{
    public class EquipmentDto
    {
        [Required]
        public int Id { get; set; }
        public String Name { get; set; } = string.Empty;
        public String Description { get; set; } = string.Empty;
        public CategoryEnum Category { get; set; }
        public bool IsActive { get; set; }     
        public int MinStock { get; set; }      
        public int Stock { get; set; }
    }

    public class CreateEquipmentDto
    {
        [Required]
        public String Name { get; set; } = string.Empty;
        public String Description { get; set; } = string.Empty;
        public CategoryEnum Category { get; set; }
        public int MinStock { get; set; }      
        public int Stock { get; set; }
    }

     public class UpdateEquipmentDto
    {
        [Required]
        public String? Name { get; set; }
        public String? Description { get; set; }
        public CategoryEnum? Category { get; set; }
        public int? MinStock { get; set; }      
        public int? Stock { get; set; }
    }
}
