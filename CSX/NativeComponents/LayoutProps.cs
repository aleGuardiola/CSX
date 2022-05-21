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
        [NativeAttribute(NativeAttribute.AlignContent)]
        public AlignContent? AlignContent { get; init; } = NativeComponents.AlignContent.FlexStart;

        [NativeAttribute(NativeAttribute.AlignItems)]
        public AlignItems? AlignItems { get; init; }

        [NativeAttribute(NativeAttribute.AlignSelf)]
        public AlignSelf? AlignSelf { get; init; }

        [NativeAttribute(NativeAttribute.AspectRatio)]
        public float? AspectRatio { get; init; }

        [NativeAttribute(NativeAttribute.Bottom)]
        public float? Bottom { get; init; }

        [NativeAttribute(NativeAttribute.Direction)]
        public Direction? Direction { get; init; }

        [NativeAttribute(NativeAttribute.Display)]
        public Display? Display { get; init; }

        [NativeAttribute(NativeAttribute.End)]
        public float? End { get; init; }

        [NativeAttribute(NativeAttribute.Flex)]
        public float? Flex { get; init; }

        [NativeAttribute(NativeAttribute.FlexBasis)]
        public float? FlexBasis { get; init; }

        [NativeAttribute(NativeAttribute.FlexDirection)]
        public FlexDirection? FlexDirection { get; init; } = NativeComponents.FlexDirection.Column;

        [NativeAttribute(NativeAttribute.FlexGrow)]
        public float? FlexGrow { get; init; }

        [NativeAttribute(NativeAttribute.FlexShrink)]
        public float? FlexShrink { get; init; } = 0;

        [NativeAttribute(NativeAttribute.FlexWrap)]
        public FlexWrap? FlexWrap { get; init; }

        [NativeAttribute(NativeAttribute.Height)]
        public float? Height { get; init; }

        [NativeAttribute(NativeAttribute.JustifyContent)]
        public JustifyContent? JustifyContent { get; init; }

        [NativeAttribute(NativeAttribute.Left)]
        public float? Left { get; init; }

        [NativeAttribute(NativeAttribute.Margin)]
        public float? Margin { get; init; }

        [NativeAttribute(NativeAttribute.MarginBottom)]
        public float? MarginBottom { get; init; }

        [NativeAttribute(NativeAttribute.MarginEnd)]
        public float? MarginEnd { get; init; }

        [NativeAttribute(NativeAttribute.MarginHorizontal)]
        public float? MarginHorizontal { get; init; }

        [NativeAttribute(NativeAttribute.MarginLeft)]
        public float? MarginLeft { get; init; }

        [NativeAttribute(NativeAttribute.MarginRight)]
        public float? MarginRight { get; init; }

        [NativeAttribute(NativeAttribute.MarginStart)]
        public float? MarginStart { get; init; }

        [NativeAttribute(NativeAttribute.MarginTop)]
        public float? MarginTop { get; init; }

        [NativeAttribute(NativeAttribute.MarginVertical)]
        public float? MarginVertical { get; init; }

        [NativeAttribute(NativeAttribute.MaxHeight)]
        public float? MaxHeight { get; init; }

        [NativeAttribute(NativeAttribute.MaxWidth)]
        public float? MaxWidth { get; init; }

        [NativeAttribute(NativeAttribute.MinHeight)]
        public float? MinHeight { get; init; }

        [NativeAttribute(NativeAttribute.MinWidth)]
        public float? MinWidth { get; init; }

        [NativeAttribute(NativeAttribute.Overflow)]
        public Overflow? Overflow { get; init; }

        [NativeAttribute(NativeAttribute.Padding)]
        public float? Padding { get; init; }

        [NativeAttribute(NativeAttribute.PaddingBottom)]
        public float? PaddingBottom { get; init; }

        [NativeAttribute(NativeAttribute.PaddingEnd)]
        public float? PaddingEnd { get; init; }

        [NativeAttribute(NativeAttribute.PaddingHorizontal)]
        public float? PaddingHorizontal { get; init; }

        [NativeAttribute(NativeAttribute.PaddingLeft)]
        public float? PaddingLeft { get; init; }

        [NativeAttribute(NativeAttribute.PaddingRight)]
        public float? PaddingRight { get; init; }

        [NativeAttribute(NativeAttribute.PaddingStart)]
        public float? PaddingStart { get; init; }

        [NativeAttribute(NativeAttribute.PaddingTop)]
        public float? PaddingTop { get; init; }

        [NativeAttribute(NativeAttribute.PaddingVertical)]
        public float? PaddingVertical { get; init; }

        [NativeAttribute(NativeAttribute.Position)]
        public Position? Position { get; init; }

        [NativeAttribute(NativeAttribute.Right)]
        public float? Right { get; init; }

        [NativeAttribute(NativeAttribute.Start)]
        public float? Start { get; init; }

        [NativeAttribute(NativeAttribute.Top)]
        public float? Top { get; init; }

        [NativeAttribute(NativeAttribute.Width)]
        public float? Width { get; init; }

        [NativeAttribute(NativeAttribute.ZIndex)]
        public float? ZIndex { get; init; }
    }
}
