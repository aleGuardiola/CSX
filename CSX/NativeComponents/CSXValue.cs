using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.NativeComponents
{
    public enum CSXUnit
    {
        Undefined,
        Point,
        Percent,
        Auto,
    }

    public static class CSXYogaConstants
    {
        public const float Undefined = float.NaN;

        public static bool IsUndefined(float value)
        {
            return float.IsNaN(value);
        }

        public static bool IsUndefined(CSXValue value)
        {
            return value.Unit == CSXUnit.Undefined;
        }
    }

    public struct CSXValue
    {
        private float value;
        private CSXUnit unit;

        public CSXUnit Unit
        {
            get
            {
                return unit;
            }
        }

        public float Value
        {
            get
            {
                return value;
            }
        }

        public static CSXValue Point(float value)
        {
            return new CSXValue
            {
                value = value,
                unit = CSXYogaConstants.IsUndefined(value) ? CSXUnit.Undefined : CSXUnit.Point
            };
        }

        public bool Equals(CSXValue other)
        {
            return Unit == other.Unit && (Value.Equals(other.Value) || Unit == CSXUnit.Undefined);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CSXValue val && Equals(val);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Value.GetHashCode() * 397) ^ (int)Unit;
            }
        }

        public static CSXValue Undefined()
        {
            return new CSXValue
            {
                value = CSXYogaConstants.Undefined,
                unit = CSXUnit.Undefined
            };
        }

        public static CSXValue Auto()
        {
            return new CSXValue
            {
                value = 0f,
                unit = CSXUnit.Auto
            };
        }

        public static CSXValue Percent(float value)
        {
            return new CSXValue
            {
                value = value,
                unit = CSXYogaConstants.IsUndefined(value) ? CSXUnit.Undefined : CSXUnit.Percent
            };
        }

        public static implicit operator CSXValue(float pointValue)
        {
            return Point(pointValue);
        }

        public static implicit operator CSXValue(string value)
        {
            if(value.Last() == '%')
            {
                return Percent(float.Parse(value.Substring(0, value.Length - 1)));
            }
            else
            {
                return Point(float.Parse(value));
            }            
        }
    }
}
