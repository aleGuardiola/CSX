using CSX.Rendering;
using System.Text;

namespace CSX.Web;

public class CSXWebDom : IDOM
{
    Dictionary<Guid, WebNode> _nodes = new Dictionary<Guid, WebNode>()
        {
            {
                Guid.Empty,
                new WebNode()
                {
                    Id = Guid.Empty,
                }
            }
        };

    public void AppendChild(Guid parent, Guid child)
    {
        try
        {
            _nodes[parent].Children.Add(_nodes[child]);
            CsxJsInterop.AttachElement(parent.ToString(), child.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }

    public Guid CreateElement(string name)
    {
        try
        {
            var node = new WebNode();
            node.Id = Guid.NewGuid();
            node.Name = name;
            _nodes[node.Id] = node;

            var htmlTag = name switch
            {
                "Image" => "img",
                _ => "div"
            };

            CsxJsInterop.CreateElement(htmlTag, node.Id.ToString());

            return node.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }

    public void DestroyElement(Guid id)
    {
        try
        {
            _nodes.Remove(id);
            CsxJsInterop.DestroyElement(id.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }

    public string? GetAttribute(Guid id, string name)
    {
        try
        {
            if (_nodes[id].Attributes.TryGetValue(name, out var value))
            {
                return value;
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }

    public Guid GetRootElement()
    {
        return Guid.Empty;
    }

    public bool HasChild(Guid parent, Guid child)
    {
        try
        {
            return _nodes[parent].Children.Any(x => x.Id == child);
        }        
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }

    public void Remove(Guid id)
    {
        try
        {
            var index = _nodes[id].Parent?.Children.IndexOf(_nodes[id]);
            _nodes[id].Parent?.Children.RemoveAt(index ?? throw new Exception("Fatal error"));
            CsxJsInterop.RemoveElement(id.ToString());

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void SetAttribute(Guid id, string name, string? value)
    {
        try
        {
            if (value == null)
            {
                _nodes[id].Attributes.Remove(name);
            }
            else
            {
                _nodes[id].Attributes[name] = value;
            }

            // style attribute
            if (name.Contains("Style."))
            {
                var style = _nodes[id].HtmlStyle;

                var propName = name.Split('.').Last();
                var cssKey = GetCssProperty(propName);

                if (value == null)
                {
                    if (style.ContainsKey(cssKey))
                    {
                        style.Remove(cssKey);
                    }
                }
                else
                {
                    style[cssKey] = GetCssValue(cssKey, value);
                }

                var css = string.Join("", style.Select(s => $"{s.Key}: {s.Value};"));
                CsxJsInterop.SetElementAttribute(id.ToString(), "style", css);
            }
            else
            {
                if(name == "TextContent")
                {
                    if(value != null)
                    {
                        CsxJsInterop.SetElementText(id.ToString(), value);
                    }                    
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        
    }

    static string GetCssValue(string key, string value)
    {
        if (value == "Coulumn")
            return "column";

        return key switch
        {
            "width"
            or "height"
            or "border-bottom-right-radius"
            or "border-bottom-left-radius"
            or "border-bottom-width"
            or "border-left-width"
            or "border-radius"
            or "border-right-width"
            or "border-top-right-radius"
            or "border-top-right-radius"
            or "border-top-left-radius"
            or "border-top-width"
            or "border-width"            
            or "bottom"
            or "right"
            or "left"
            or "margin"
            or "margin-bottom"
            or "margin-right"
            or "margin-left"
            or "margin-top"
            or "max-height"
            or "max-width"
            or "min-height"
            or "min-width"
            or "padding"
            or "padding-bottom"
            or "padding-right"
            or "padding-left"
            or "padding-top"
            or "max-width"
            or "max-width"
            or "font-size"
            => value + "px",

            string k when k.Contains("color") => value,

            _ => GetCssValue(value)
        };
    }

    static string GetCssValue(string enumValue)
    {
        // convert from cammel case to css whatever case
        // inspired by https://stackoverflow.com/questions/63055621/how-to-convert-camel-case-to-snake-case-with-two-capitals-next-to-each-other

        if (enumValue.Length < 2)
        {
            return enumValue.ToLower();
        }
        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(enumValue[0]));
        for (int i = 1; i < enumValue.Length; ++i)
        {
            char c = enumValue[i];
            if (char.IsUpper(c))
            {
                sb.Append('-');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    static string GetCssProperty(string csxProperty)
    {
        return csxProperty switch
        {            
            "BackgroundColor" => "background-color",
            "BorderBottomColor" => "border-bottom-color",
            "BorderBottomEndRadius" => "border-bottom-right-radius",
            "BorderBottomLeftRadius" => "border-bottom-left-radius",
            "BorderBottomRightRadius" => "border-bottom-right-radius",
            "BorderBottomStartRadius" => "border-bottom-left-radius",
            "BorderBottomWidth" => "border-bottom-width",
            "BorderColor" => "border-color",
            "BorderEndColor" => "border-right-color",
            "BorderLeftColor" => "border-left-color",
            "BorderLeftWidth" => "border-left-width",
            "BorderRadius" => "border-radius",
            "BorderRightColor" => "border-right-color",
            "BorderRightWidth" => "border-right-width",
            "BorderStartColor" => "border-left-color",
            "BorderStyle" => "border-style",
            "BorderTopColor" => "border-top-color",
            "BorderTopEndRadius" => "border-top-right-radius",
            "BorderTopLeftRadius" => "border-top-left-radius",
            "BorderTopRightRadius" => "border-top-right-radius",
            "BorderTopStartRadius" => "border-top-left-radius",
            "BorderTopWidth" => "border-top-width",
            "BorderWidth" => "border-width",
            "Opacity" => "opacity",
            "AlignContent" => "align-content",
            "AlignItems" => "align-items",
            "AlignSelf" => "align-self",
            "AspectRatio" => "TODO",
            "Bottom" => "bottom",
            "Direction" => "direction",
            "Display" => "display",
            "End" => "right",
            "Flex" => "flex",
            "FlexBasis" => "flex-basis",
            "FlexDirection" => "flex-direction",
            "FlexGrow" => "flex-grow",
            "FlexShrink" => "flex-shrink",
            "FlexWrap" => "flex-wrap",
            "Height" => "height",
            "JustifyContent" => "justify-content",
            "Left" => "left",
            "Margin" => "margin",
            "MarginBottom" => "margin-bottom",
            "MarginEnd" => "margin-right",
            "MarginHorizontal" => "TODO",
            "MarginLeft" => "margin-left",
            "MarginRight" => "margin-right",
            "MarginStart" => "margin-left",
            "MarginTop" => "margin-top",
            "MarginVertical" => "TODO",
            "MaxHeight" => "max-height",
            "MaxWidth" => "max-width",
            "MinHeight" => "min-height",
            "MinWidth" => "min-width",
            "Overflow" => "overflow",
            "Padding" => "padding",
            "PaddingBottom" => "padding-bottom",
            "PaddingEnd" => "padding-right",
            "PaddingHorizontal" => "TOD",
            "PaddingLeft" => "padding-left",
            "PaddingRight" => "padding-right",
            "PaddingStart" => "padding-left",
            "PaddingTop" => "padding-top",
            "PaddingVertical" => "TODO",
            "Position" => "position",
            "Right" => "right",
            "Start" => "left",
            "Top" => "top",
            "Width" => "width",
            "ZIndex" => "z-index",

            // Text styles
            "Color" => "color",
            "FontSize" => "font-size",

            _ => throw new NotImplementedException("The style is not supported")
        };
    }

}

internal class WebNode
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Dictionary<string, string> HtmlStyle { get; set; } = new Dictionary<string, string>();
    public WebNode? Parent { get; set; }
    public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();
    public List<WebNode> Children { get; } = new List<WebNode>();
}


