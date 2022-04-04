using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CSX.Compilation
{
    public static class CsxCompiler
    {
        public static string ToCSharp(XmlNode xmlNode, Func<string, string> PropsTypeNameResolver)
        {

            if (xmlNode.Name == "#text")
            {
                return $"ComponentFactory.CreateElement<StringComponent, StringProps>(new($\"{xmlNode.InnerText}\"), new List<VirtualComponent>())";
            }

            var propType = PropsTypeNameResolver(xmlNode.Name);

            return $@"
        ComponentFactory.CreateElement<{xmlNode.Name}, {propType}>(new() {{ {AttrToCSharp(xmlNode.Attributes)} }}, new List<VirtualComponent>(){{
           { string.Join(",\n", xmlNode.ChildNodes.Cast<XmlNode>().Select(node => ToCSharp(node, PropsTypeNameResolver)))} 
        }})
    ";
        }

        static string AttrToCSharp(XmlAttributeCollection attributes)
        {
            var result = new List<string>();
            foreach (var attr in attributes.Cast<XmlNode>())
            {
                var name = attr.Name;
                var value = attr.Value;

                if (value.StartsWith("@"))
                {
                    var codeLiteral = value.Substring(1);
                    result.Add($"{name}={codeLiteral}");
                }
                else
                {
                    result.Add($"{name}=\"{value}\"");
                }
            }

            return string.Join(", ", result);
        }

    }
}
