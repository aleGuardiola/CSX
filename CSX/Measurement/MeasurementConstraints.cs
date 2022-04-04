using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Measurement
{
    public record MeasurementConstraints(
        MeasureMode WidthMode,
        MeasureMode HeightMode,
        Size size);
}
