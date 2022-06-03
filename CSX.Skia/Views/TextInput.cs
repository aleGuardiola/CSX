using CSX.NativeComponents;
using CSX.Rendering;
using CSX.Skia.Events;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Views
{
    public class TextInput : View, IViewWithText
    {
        public string TextContent { get => _textNode.Text; set => _textNode.Text = value; }

        TextNode _textNode;

        public TextInput(ulong id) : base(id)
        {
            // SetAttribute(NativeAttribute.Padding, 5f);
            SetAttribute(NativeAttribute.BorderColor, Color.Black);
            SetAttribute(NativeAttribute.BorderWidth, 1f);

            _textNode = new TextNode(id);
            AppendView(_textNode);
        }

    }
}
