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
        public static (string code, IEnumerable<(string type, string name)> referencesName) ToCSharp(XmlNode xmlNode, Func<string, string> PropsTypeNameResolver)
        {
            var references = new List<(string type, string name)>();
            var code = ToCSharp(xmlNode, PropsTypeNameResolver, references);

            return (code, references);
        }

        public static string ToCSharp(XmlNode xmlNode, Func<string, string> PropsTypeNameResolver, List<(string type, string name)> references)
        {

            if (xmlNode.Name == "#text")
            {
                return $"ComponentFactory.CreateElement<StringComponent, StringProps>(new($\"{xmlNode.InnerText}\"), new List<Element>())";
            }

            var propType = PropsTypeNameResolver(xmlNode.Name);

            var refAttr = xmlNode.Attributes.Cast<XmlNode>().FirstOrDefault(x => x.Name == "_Ref");
            
            if(refAttr != null)
            {
                references.Add((xmlNode.Name, refAttr.Value));
            }

            return $@"
        {(refAttr == null ? "" : $"{refAttr.Value}Element = " )} ComponentFactory.CreateElement<{xmlNode.Name}, {propType}>(new() {{ {AttrToCSharp(xmlNode.Attributes)} }}, new List<Element>(){{
           { string.Join(",\n", xmlNode.ChildNodes.Cast<XmlNode>().Select(node => ToCSharp(node, PropsTypeNameResolver, references)))} 
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
