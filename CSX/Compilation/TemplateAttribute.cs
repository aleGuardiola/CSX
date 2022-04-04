using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Compilation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TemplateAttribute : Attribute
    {
        public string Name { get; set; }
        public TemplateAttribute( string name)
        {
            Name = name;
        }
    }
}
