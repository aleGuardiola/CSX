using CSX.Components;

namespace CSX.NativeComponents
{
    public enum AlignContent
    {
        FlexStart,
        FlexEnd,
        Center,
        Stretch,
        SpaceBetween,
        SpaceAround
    }
    public enum AlignItems
    {
        FlexStart,
        FlexEnd,
        Center,
        Stretch,
        Baseline,
    }

    public enum AlignSelf
    {
        Auto,
        FlexStart,
        FlexEnd,
        Center,
        Stretch,
        Baseline,
    }

    public enum Direction
    { 
        Inherit,
        Ltr,
        Rtl
    }

    public enum Display
    {
        Flex,
        None
    }
    public enum FlexDirection
    {
        Row,
        RowReverse,
        Column,
        ColumnReverse
    }    
    public enum FlexWrap
    {
        NoWrap,
        Wrap,
        WrapReverse
    }
    public enum JustifyContent
    {
        FlexStart,
        FlexEnd,
        Center,
        SpaceBetween,
        SpaceAround,
        SpaceEvenly
    }
    public enum Overflow
    {
        Visible,
        Hidden,
        Scroll
    }
    public enum Position
    {        
        Relative,
        Absolute     
    }
    public record LayoutStyleProps : Props
    {
        public AlignContent? AlignContent { get; init; } = NativeComponents.AlignContent.FlexStart;
        public AlignItems? AlignItems { get; init; }
        public AlignSelf? AlignSelf { get; init; }
        public double? AspectRatio { get; init; }
        public double? Bottom { get; init; }
        public Direction? Direction { get; init; }
        public Display? Display { get; init; }
        public double? End { get; init; }
        public double? Flex { get; init; }
        public double? FlexBasis { get; init; }
        public FlexDirection? FlexDirection { get; init; } = NativeComponents.FlexDirection.Column;
        public double? FlexGrow { get; init; }
        public double? FlexShrink { get; init; } = 0;
        public FlexWrap? FlexWrap { get; init; }
        public double? Height { get; init; }
        public JustifyContent? JustifyContent { get; init; }
        public double? Left { get; init; }
        public double? Margin { get; init; }
        public double? MarginBottom { get; init; }
        public double? MarginEnd { get; init; }
        public double? MarginHorizontal { get; init; }
        public double? MarginLeft { get; init; }
        public double? MarginRight { get; init; }
        public double? MarginStart { get; init; }
        public double? MarginTop { get; init; }
        public double? MarginVertical { get; init; }
        public double? MaxHeight { get; init; }
        public double? MaxWidth { get; init; }
        public double? MinHeight { get; init; }
        public double? MinWidth { get; init; }
        public Overflow? Overflow { get; init; }
        public double? Padding { get; init; }
        public double? PaddingBottom { get; init; }
        public double? PaddingEnd { get; init; }
        public double? PaddingHorizontal { get; init; }
        public double? PaddingLeft { get; init; }
        public double? PaddingRight { get; init; }
        public double? PaddingStart { get; init; }
        public double? PaddingTop { get; init; }
        public double? PaddingVertical { get; init; }
        public Position? Position { get; init; }
        public double? Right { get; init; }
        public double? Start { get; init; }
        public double? Top { get; init; }
        public double? Width { get; init; }
        public double? ZIndex { get; init; }
    }
}
