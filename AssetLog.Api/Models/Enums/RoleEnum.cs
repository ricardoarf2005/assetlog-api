using System.ComponentModel;

namespace AssetLog.Api.Models.Enums
{
    public enum RoleEnum
    {
        [Description("Trainee")]
        TRAINEE,

        [Description("Employee")]
        EMPLOYEE,

        [Description("Manager")]
        MANAGER,
    }
}
