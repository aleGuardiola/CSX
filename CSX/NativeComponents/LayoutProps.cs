using CSX.Components;
using CSX.Rendering;

namespace CSX.NativeComponents
{
    public enum AlignContent
    {
        FlexStart = 0,
        FlexEnd,
        Center,
        Stretch,
        SpaceBetween,
        SpaceAround
    }
    public enum AlignItems
    {
        FlexStart = 0,
        FlexEnd,
        Center,
        Stretch,
        Baseline,
    }

    public enum AlignSelf
    {
        Auto = 0,
        FlexStart,
        FlexEnd,
        Center,
        Stretch,
        Baseline,
    }

    public enum Direction
    { 
        Inherit = 0,
        Ltr,
        Rtl
    }

    public enum Display
    {
        Flex = 0,
        None
    }
    public enum FlexDirection
    {
        Row = 0,
        RowReverse,
        Column,
        ColumnReverse
    }    
    public enum FlexWrap
    {
        NoWrap = 0,
        Wrap,
        WrapReverse
    }
    public enum JustifyContent
    {
        FlexStart = 0,
        FlexEnd,
        Center,
        SpaceBetween,
        SpaceAround,
        SpaceEvenly
    }
    public enum Overflow
    {
        Visible = 0,
        Hidden,
        Scroll
    }
    public enum Position
    {        
        Relative = 0,
        Absolute     
    }
    public record LayoutStyleProps : Props
    {
        public AlignContent? AlignContent { get; init; } = NativeComponents.AlignContent.FlexStart;

        public AlignItems? AlignItems { get; init; }

        public AlignSelf? AlignSelf { get; init; }

        public float? AspectRatio { get; init; }

        public float? Bottom { get; init; }

        public Direction? Direction { get; init; }

        public Display? Display { get; init; }

        public float? End { get; init; }

        public float? Flex { get; init; }

        public CSXValue? FlexBasis { get; init; }

        public FlexDirection? FlexDirection { get; init; } = NativeComponents.FlexDirection.Column;

        public float? FlexGrow { get; init; }

        public float? FlexShrink { get; init; } = 0;

        public FlexWrap? FlexWrap { get; init; }

        public CSXValue? Height { get; init; }

        public JustifyContent? JustifyContent { get; init; }

        public float? Left { get; init; }

        public float? Margin { get; init; }

        public float? MarginBottom { get; init; }

        public float? MarginEnd { get; init; }

        public float? MarginHorizontal { get; init; }

        public float? MarginLeft { get; init; }

        public float? MarginRight { get; init; }

        public float? MarginStart { get; init; }

        public float? MarginTop { get; init; }

        public float? MarginVertical { get; init; }

        public CSXValue? MaxHeight { get; init; }

        public CSXValue? MaxWidth { get; init; }

        public CSXValue? MinHeight { get; init; }

        public CSXValue? MinWidth { get; init; }

        public Overflow? Overflow { get; init; }

        public float? Padding { get; init; }

        public float? PaddingBottom { get; init; }

        public float? PaddingEnd { get; init; }

        public float? PaddingHorizontal { get; init; }

        public float? PaddingLeft { get; init; }

        public float? PaddingRight { get; init; }

        public float? PaddingStart { get; init; }

        public float? PaddingTop { get; init; }

        public float? PaddingVertical { get; init; }

        public Position? Position { get; init; }

        public float? Right { get; init; }

        public float? Start { get; init; }

        public float? Top { get; init; }

        public CSXValue? Width { get; init; }

        public int? ZIndex { get; init; }
    }
}
