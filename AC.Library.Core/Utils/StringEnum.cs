using AC.Library.Core.Interfaces;

namespace AC.Library.Core.Utils
{
    public class StringEnum : IParameter
    {
        public string Value { get; set; }
 
        public StringEnum(string value) => Value = value;
 
        public static bool operator ==(StringEnum enumA, StringEnum enumB) => enumA.Value == enumB.Value;
        public static bool operator !=(StringEnum enumA, StringEnum enumB) => enumA.Value != enumB.Value;
 
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
 
            if (ReferenceEquals(obj, null))
                return false;
            
            if (!(obj is StringEnum))
                return false;

            var enumB = (StringEnum)obj;
 
            return Value.Equals(enumB.Value);
        }
 
        public override int GetHashCode() => Value.GetHashCode();
    }
}