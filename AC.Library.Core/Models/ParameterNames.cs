using AC.Library.Core.Utils;

namespace AC.Library.Core.Models
{
    public class PowerParam : StringEnum 
    {
        public PowerParam(string value) : base(value) {}
        public static PowerParam Power => new PowerParam("Pow");
    }

    public class ModeParam : StringEnum
    {
        public ModeParam(string value) : base(value) {}
        public static ModeParam Mode => new ModeParam("Mod");
    }

    public class TemperatureParam : StringEnum
    {
        public TemperatureParam(string value) : base(value) {}
        public static TemperatureParam Temperature => new TemperatureParam("SetTem");
    }

    public class FanSpeedParam : StringEnum
    {
        public FanSpeedParam(string value) : base(value) {}
        public static FanSpeedParam FanSpeed => new FanSpeedParam("WdSpd");
    }

    public class AirModeParam : StringEnum
    {
        public AirModeParam(string value) : base(value) {}
        public static AirModeParam AirMode => new AirModeParam("Air");
    }

    public class XfanModeParam : StringEnum
    {
        public XfanModeParam(string value) : base(value) {}
        public static XfanModeParam XFanMode => new XfanModeParam("Blo");
    }

    public class HealthModeParam : StringEnum
    {
        public HealthModeParam(string value) : base(value) {}
        public static HealthModeParam HealthMode => new HealthModeParam("Health");
    }

    public class SleepModeParam : StringEnum
    {
        public SleepModeParam(string value) : base(value) {}
        public static SleepModeParam SleepMode => new SleepModeParam("SwhSlp");
    }

    public class LightParam : StringEnum
    {
        public LightParam(string value) : base(value) {}
        public static LightParam Light => new LightParam("Lig");
    }

    public class VerticalSwingParam : StringEnum
    {
        public VerticalSwingParam(string value) : base(value) {}
        public static VerticalSwingParam VerticalSwing => new VerticalSwingParam("SwUpDn");
    }

    public class QuietModeParam : StringEnum
    {
        public QuietModeParam(string value) : base(value) {}
        public static QuietModeParam QuietMode => new QuietModeParam("Quiet");
    }

    public class TurboModeParam : StringEnum
    {
        public TurboModeParam(string value) : base(value) {}
        public static TurboModeParam TurboMode => new TurboModeParam("Tur");
    }

    public class EnergySavingModeParam : StringEnum
    {
        public EnergySavingModeParam(string value) : base(value) {}
        public static EnergySavingModeParam EnergySavingMode => new EnergySavingModeParam("SvSt");
    }

    public class TemperatureUnitParam : StringEnum
    {
        public TemperatureUnitParam(string value) : base(value) {}
        public static TemperatureUnitParam TemperatureUnit => new TemperatureUnitParam("TemUn");
    }
}


