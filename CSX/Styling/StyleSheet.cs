using CSX.NativeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Styling
{    
    public class StyleSheet<T>
    {
        public T Styles { get; }

        Dictionary<string, Dictionary<string, object?>> _styles = new Dictionary<string, Dictionary<string, object?>>();

        public StyleSheet(T definition, Dictionary<string, Dictionary<string, object?>> styles)
        {
            _styles = styles;
            Styles = definition;
        }

        public TStyle Apply<TStyle>(Expression<Func<T, object?>> propery) where TStyle : ViewStyleProps
        {
            var expression = (MemberExpression)propery.Body;
            string name = expression.Member.Name;

            var definition = _styles[name];

            return StyleSheet.GetStyles<TStyle>(definition);
        }

        public ViewStyleProps Apply(Expression<Func<T, object?>> property)
            => Apply<ViewStyleProps>(property);

    }

    public static class StyleSheet
    {

        public static StyleSheet<TD> Create<TD>(TD definitions)
        {
            _ = definitions ?? throw new ArgumentNullException(nameof(definitions));

            var definitionsDict = new Dictionary<string, Dictionary<string, object?>>();

            var type = definitions.GetType();
            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
                var propValue = prop.GetValue(definitions);
                var propProperties = prop.PropertyType.GetProperties();

                var definitionDict = new Dictionary<string, object?>();
                foreach (var propProperty in propProperties)
                {
                    definitionDict.Add(propProperty.Name, propProperty.GetValue(propValue));
                }
                definitionsDict.Add(prop.Name, definitionDict);
            }

            return new StyleSheet<TD>(definitions, definitionsDict);
        }

        public static TStyle GetStyles<TStyle>(Dictionary<string, object?> definition) where TStyle : ViewStyleProps
        {
            var instance = Activator.CreateInstance<TStyle>();

            foreach (var pair in definition)
            {
                SetObjectProperty(instance, pair.Key, pair.Value);
            }

            return instance;
        }


        static void SetObjectProperty(object obj, string name, object? value)
        {
            var type = obj.GetType();
            var property = type.GetProperty(name);
            if (property == null)
            {
                return;
            }

            property.SetValue(obj, value);
        }



    }


}
