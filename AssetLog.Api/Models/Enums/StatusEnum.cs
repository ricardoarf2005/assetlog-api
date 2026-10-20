using System.ComponentModel;

namespace AssetLog.Api.Models.Enums
{
    public enum StatusEnum
    {
        [Description("Opened")]
        OPENED,

        [Description("Pending")]
        PENDING,

        [Description("Approved")]
        APPROVED,

        [Description("Ongoing")]
        ONGOING,

        [Description("Rejected")]
        REJECTED,

        [Description("Blocked")]
        BLOCKED,

        [Description("Returned")]
        RETURNED,

        [Description("Return Rejected")]
        RETURNREJECTED,

        [Description("Closed")]
        CLOSED,
    }
}
