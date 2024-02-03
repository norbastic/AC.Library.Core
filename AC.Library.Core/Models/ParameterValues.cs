using AC.Library.Core.Interfaces;

namespace AC.Library.Core.Models
{
    public enum PowerValues {
        Off = 0,
        On = 1
    }

    public class PowerParameterValue : IParameterValue
    {
        private readonly PowerValues _value;
        public PowerParameterValue(PowerValues value) {
            _value = value;
        }
        public int Value => (int) _value;
    }

    public enum ModeValues {
        Auto = 0,
        Cool = 1,
        Dry = 2,
        Fan = 3,
        Heat = 4
    }

    public class ModeParameterValue : IParameterValue
    {
        private readonly ModeValues _value;
        public ModeParameterValue(ModeValues value) {
            _value = value;
        }
        public int Value => (int) _value;
    }

    public enum TemperatureValues
    {
        _16 = 16,
        _17 = 17,
        _18 = 18,
        _19 = 19,
        _20 = 20,
        _21 = 21,
        _22 = 22,
        _23 = 23,
        _24 = 24,
        _25 = 25,
        _26 = 26,
        _27 = 27,
        _28 = 28,
        _29 = 29,
        _30 = 30,
        _31 = 31,
        _32 = 32,
        _33 = 33,
        _34 = 34,
        _35 = 35
    }

    public class TempParameterValue : IParameterValue
    {
        private readonly TemperatureValues _value;
        public TempParameterValue(TemperatureValues value) {
            _value = value;
        }
        public int Value => (int) _value;
    }

    public enum TempUnitValues {
        Celsius = 0,
        Fahrenheit = 1
    }

    public class TempUnitParameterValue : IParameterValue
    {
        private readonly TempUnitValues _value;
        public TempUnitParameterValue(TempUnitValues value) {
            _value = value;
        }
        public int Value => (int) _value;
    }

    public enum FanSpeedValues {
        Auto = 0,
        Low = 1,
        MediumLow = 2,
        Medium = 3,
        MediumHigh = 4,
        High = 5
    }

    public class FanSpeedParameterValue : IParameterValue
    {
        private readonly FanSpeedValues _value;
        public FanSpeedParameterValue(FanSpeedValues value) {
            _value = value;
        }
        public int Value => (int) _value;
    }

    public enum AirValues {
        Off = 0,
        Inside = 1,
        Outside = 2,
        Mode3 = 3
    }

    public class AirParameterValue : IParameterValue
    {
        private readonly AirValues _value;
        public AirParameterValue(AirValues value) {
            _value = value;
        }
        public int Value => (int) _value;
    }

    public enum HealthValues {
        Off = 0,
        On = 1,
    }

    public class HealthParameterValue : IParameterValue
    {
        private readonly HealthValues _value;
        public HealthParameterValue(HealthValues value) {
            _value = value;
        }
        public int Value => (int) _value;
    }

    public enum SleepValues {
        Off = 0,
        On = 1,
    }

    public class SleepParameterValue : IParameterValue
    {
        private readonly SleepValues _value;
        public SleepParameterValue(SleepValues value) {
            _value = value;
        }
        public int Value => (int) _value;
    }

    public enum LightValues {
        Off = 0,
        On = 1,
    }

    public class LightParameterValue : IParameterValue
    {
        private readonly LightValues _value;
        public LightParameterValue(LightValues value) {
            _value = value;
        }
        public int Value => (int) _value;
    }

    public enum SwingHorizontalValues {
        Default = 0,
        Full = 1,
        FixedLeft = 2,
        FixedMidLeft = 3,
        FixedMid = 4,
        FixedMidRight = 5,
        FixedRight = 6,
        FullAlt = 7,
    }

    public class SwingHorizontalParameterValue : IParameterValue
    {
        private readonly SwingHorizontalValues _value;
        public SwingHorizontalParameterValue(SwingHorizontalValues value) {
            _value = value;
        }
        public int Value => (int) _value;
    }

    public enum SwingVerticalValues {
        Default = 0,
        Full = 1,
        FixedTop = 2,
        FixedMidTop = 3,
        FixedMid = 4,
        FixedMidBottom = 5,
        FixedBottom = 6,
        SwingBottom = 7,
        SwingMidBottom = 8,
        SwingMid = 9,
        SwingMidTop = 10,
        SwingTop = 11,
    }

    public class SwingVerticalParameterValue : IParameterValue
    {
        private readonly SwingVerticalValues _value;
        public SwingVerticalParameterValue(SwingVerticalValues value)
        {
            _value = value;
        }
        public int Value => (int)_value;
    }

    public enum QuietValues {
        Off = 0,
        Mode1 = 1,
        Mode2 = 2,
        Mode3 = 3,
    }

    public class QuietParameterValue : IParameterValue
    {
        private readonly QuietValues _value;
        public QuietParameterValue(QuietValues value)
        {
            _value = value;
        }
        public int Value => (int)_value;
    }

    public enum TurboValues {
        Off = 0,
        On = 1,
    }

    public class TurboParameterValue : IParameterValue
    {
        private readonly TurboValues _value;
        public TurboParameterValue(TurboValues value)
        {
            _value = value;
        }
        public int Value => (int)_value;
    }

    public enum PowerSaveValues {
        Off = 0,
        On = 1,
    }

    public class PowerSaveParameterValue : IParameterValue
    {
        private readonly PowerSaveValues _value;
        public PowerSaveParameterValue(PowerSaveValues value)
        {
            _value = value;
        }
        public int Value => (int)_value;
    }

    public enum SafetyHeatingValues {
        Off = 0,
        On = 1,
    }

    public class SafetyHeatingParameterValue : IParameterValue
    {
        private readonly SafetyHeatingValues _value;
        public SafetyHeatingParameterValue(SafetyHeatingValues value)
        {
            _value = value;
        }
        public int Value => (int)_value;
    }
}

