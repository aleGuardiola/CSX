
//using CSX.Components;
//using CSX.NativeComponents;

//namespace CSX.Lab
//{
//    public partial class ComponentTest
//    {

//        private Element? MainViewElement;
//        protected View? MainView => MainViewElement?.Component as View;

//        protected override Element Render()
//        {
//            return
//        MainViewElement = ComponentFactory.CreateElement<View, ViewProps>(new() { _Ref = "MainView", Style = new() { BackgroundColor = "red", FlexDirection = FlexDirection.Column } }, new List<Element>(){

//         ComponentFactory.CreateElement<Text, TextProps>(new() {  }, new List<Element>(){
//           ComponentFactory.CreateElement<StringComponent, StringProps>(new($"{State.Name}"), new List<Element>())
//        })
//    ,

//         ComponentFactory.CreateElement<Text, TextProps>(new() {  }, new List<Element>(){
//           ComponentFactory.CreateElement<StringComponent, StringProps>(new($"{State.LastName}"), new List<Element>())
//        })

//        })
//    ;
//        }
//    }
//}

