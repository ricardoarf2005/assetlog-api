using System.ComponentModel;

namespace AssetLog.Api.Models.Enums
{
    public enum CategoryEnum
    {
        [Description("Peripherals")]
        PERIPHERALS,

        [Description("Monitors")]
        MONITORS,

        [Description("Audio")]
        AUDIO,

        [Description("Laptop")]
        LAPTOPS,

        [Description("Smartphones")]
        SMARTPHONES,

        [Description("Connectivity")]
        CONECTIVITY,

        [Description("Others")]
        OTHERS,
    }
}
