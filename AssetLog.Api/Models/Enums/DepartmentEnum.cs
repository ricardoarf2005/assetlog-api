using System.ComponentModel;

namespace AssetLog.Api.Models.Enums
{
    public enum DepartmentEnum
    {
        [Description("IT")]
        IT,

        [Description("DEV")]
        DEV,

        [Description("HR")]
        HR,

        [Description("Accounting")]
        ACCOUNTING,

        [Description("Sales")]
        SALES,

        [Description("Marketing")]
        MARKETING,
    }
}
