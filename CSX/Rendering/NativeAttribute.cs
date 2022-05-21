using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Rendering
{
    public enum NativeAttribute
    {
        // Layout
        AlignContent = 0,
        AlignItems,
        AlignSelf,
        AspectRatio,
        Bottom,
        Direction,
        Display,
        End,
        Flex,
        FlexBasis,
        FlexDirection,
        FlexGrow,
        FlexShrink,
        FlexWrap,
        Height,
        JustifyContent,
        Left,
        Margin,
        MarginBottom,
        MarginEnd,
        MarginHorizontal,
        MarginLeft,
        MarginRight,
        MarginStart,
        MarginTop,
        MarginVertical,
        MaxHeight,
        MaxWidth,
        MinHeight,
        MinWidth,
        Overflow,
        Padding,
        PaddingBottom,
        PaddingEnd,
        PaddingHorizontal,
        PaddingLeft,
        PaddingRight,
        PaddingStart,
        PaddingTop,
        PaddingVertical,
        Position,
        Right,
        Start,
        Top,
        Width,
        ZIndex,

        // View style props
        BackgroundColor,
        BorderBottomColor,
        BorderBottomEndRadius,
        BorderBottomLeftRadius,
        BorderBottomRightRadius,
        BorderBottomStartRadius,
        BorderBottomWidth,
        BorderColor,
        BorderEndColor,
        BorderLeftColor,
        BorderLeftWidth,
        BorderRadius,
        BorderRightColor,
        BorderRightWidth,
        BorderStartColor,
        BorderStyle,
        BorderTopColor,
        BorderTopEndRadius,
        BorderTopLeftRadius,
        BorderTopRightRadius,
        BorderTopStartRadius,
        BorderTopWidth,
        BorderWidth,
        Opacity,

        // Text style
        Color,
        FontFamily,
        FontSize,
        FontStyle,
        FontWeight,
        FontVariant,
        LetterSpacing,
        LineHeight,
        TextAlign,
        TextDecorationLine,
        TextShadowColor,
        TextShadowOffsetX,
        TextShadowOffsetY,
        TextShadowRadius,
        TextTransform,

        // Image
        ResizeMode,
        Source,

        // Scroll View
        ScrollPosition
    }
}
