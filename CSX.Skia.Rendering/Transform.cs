using Facebook.Yoga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Rendering
{
    public record Transform
    {        
        public AngleValue Rotate { get; init; }
        public AngleValue RotateX { get; init; }
        public AngleValue RotateY { get; init; }
        public AngleValue RotateZ { get; init; }
        public float Scale { get; init; }
        public float ScaleX { get; init; }
        public float ScaleY { get; init; }
        public YogaValue TranslateX { get; init; }
        public YogaValue TranslateY { get; init; }
        public AngleValue SkewX { get; init; }
        public AngleValue SkewY { get; init; }
    }
}
