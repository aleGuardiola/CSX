using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Rendering
{

    public enum AngleValueUnit
    {
        Undefined,
        Degrees,
        Gradians,
        Radians,
        Turns
    }

    public struct AngleValue
    {
        public AngleValueUnit Unit { get; }
        public float Value { get; }

        public AngleValue(AngleValueUnit unit, float value)
        {
            Unit = unit;
            Value = value;
        }

        public bool Equals(AngleValue other)
        {
            return Unit == other.Unit && (Value.Equals(other.Value) || Unit == AngleValueUnit.Undefined);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is AngleValue val && Equals(val);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value.GetHashCode() * 397) ^ (int)Unit;
            }
        }

    }
}
