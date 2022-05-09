// See https://aka.ms/new-console-template for more information
using CSX.Components;
using CSX.Lab;
using CSX.NativeComponents;
using Microsoft.Extensions.DependencyInjection;
using System.Xml;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components;

Console.WriteLine("Hello, World!");

var serviceCollection = new ServiceCollection();

serviceCollection.AddTransient<ComponentTest>();
serviceCollection.AddTransient<Text>();
serviceCollection.AddTransient<StringComponent>();
serviceCollection.AddTransient<View>();

var provider = serviceCollection.BuildServiceProvider();

var dom = new MemoryDOM();

var virtualThing = ComponentFactory.CreateElement<ComponentTest, TestProps>(new() { Name = "Alejandr", LastName = "Guardiola" }, new List<CSX.Components.Element>());

ComponentFactory.CreateComponent(virtualThing, provider, dom);

var component = virtualThing.Component ?? throw new Exception("Component is null");

dom.AppendToDom(component);

component.RenderView(dom);

ComponentFactory.DestroyComponent(virtualThing, dom);



//var xml = @"
//<View Style='@ new(){BackgroundColor = State.ContentColor}'>
//    <Text>Hello, World!</Text>
//</View>
//";

//XmlDocument xmlDoc = new XmlDocument();
//xmlDoc.LoadXml(xml);

//if (xmlDoc.ChildNodes.Count > 1)
//{
//    throw new InvalidOperationException("A component can only have one root component");
//}

//var root = xmlDoc.ChildNodes.Item(0);

//var propsNameResolver = (string n) => {

//    var assemblies = new Assembly[] { typeof(View).Assembly, typeof(MemoryDOM).Assembly };
//    var type = assemblies.SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == n);

//    var propType = type.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.Name.Contains("IComponent")).GetGenericArguments().First();

//    var name = propType.Name;
//    var properties = propType.GetProperties();

//    return new ObjectValueType(name, properties.Select(x => new ValuePropertySchema(x.Name, new ValueType(x.PropertyType.Name))).ToList()).Name;
//};

//var csharpCode = CsxCompiler.ToCSharp(root, propsNameResolver);

//string AttributesToCSharp()

//var razor = new ComponentTestRazor();
//var str = razor.GenerateString();
//Console.WriteLine(str);

//public record ValueType(string Name);
//public record ObjectValueType(string Name, List<ValuePropertySchema> properties) : ValueType(Name);
//public record ListValueType(string Name, ValueType type) : ValueType(Name);
//public record ValuePropertySchema(string Name, ValueType Type);


//RenderFragment renderFragment;
//renderFragment = b =>
//{
//    b.OpenComponent(1, typeof(View));
//    b.AddAttribute(2, "Style", new { BackgroundColor = "red" });
//    b.CloseComponent();    
//};

//var tree = new Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder();

//renderFragment.Invoke(tree);



Console.Read();

