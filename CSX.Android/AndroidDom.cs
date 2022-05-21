using CSX.Rendering;
using System.Reactive.Subjects;
using CSX.Native;
using Android.Views;
using static Android.Views.ViewGroup;
using Java.Util;
using Android.Graphics;

namespace CSX.Android
{
    public class AndroidDom : NativeDom<AndroidViewNode>
    {
        Activity Activity;
        //FrameLayout Root;

        public AndroidDom(Activity activity)
        {
            Activity = activity;
            //Root = new FrameLayout(activity);
            //Activity.SetContentView(Root);

            SetViewPortDimmensions(600, 600);

            Func<Task> timer = null;
            timer = async () =>
            {
                await Task.Delay(30);
                CalculateLayout();
                Task.Run(timer);
            };
            Task.Run(timer);
        }

        protected override void AppendNode(AndroidViewNode parent, AndroidViewNode child)
        {
            if(parent.Element == NativeElement.Root)
            {
                child.AndroidView.LayoutChange += (sender, args) =>
                {
                    int width = args.Right - args.Left;
                    int height = args.Bottom - args.Top;
                    SetViewPortDimmensions(width, height);
                };
                Activity.SetContentView(child.AndroidView);                
            }
        }

        protected override AndroidViewNode CreateNode(ulong id, NativeElement element)
        {            
            var view = element switch
            {                
                NativeElement.View => RenderView(),
                NativeElement.Button => RenderButton(),
                NativeElement.Image => RenderImage(),
                NativeElement.ScrollView => RenderScrollView(),
                NativeElement.Text => RenderText(),
                NativeElement.TextInput => RenderTextInput(),
                NativeElement.SkiaCanvas => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };

            return new AndroidViewNode(id, element, view);
        }

        protected override AndroidViewNode CreateRoot()
        {
            return new AndroidViewNode(0, NativeElement.Root, null);
        }

        protected override void DestroyNode(AndroidViewNode node)
        {
            node.AndroidView.Dispose();
        }

        protected override void RemoveNode(AndroidViewNode node)
        {
            (node.Parent?.AndroidView as ViewGroup)?.RemoveView(node.AndroidView);
        }

        protected override void SetNodeChildren(AndroidViewNode parent, AndroidViewNode[] children)
        {
            Activity.RunOnUiThread(() =>
            {
                var viewGroup = (parent.AndroidView as ViewGroup);

                if (viewGroup == null)
                {
                    return;
                }

                viewGroup.RemoveAllViews();

                foreach (var child in children)
                {
                    viewGroup.AddView(child.AndroidView);
                }
            });            
        }

        protected override void SetNodeStyleAttribute(AndroidViewNode node, NativeAttribute attribute, object? value)
        {
            var view = node.AndroidView;
            if(value == null)
            {
                return;
            }

            Activity.RunOnUiThread(() =>
            {
                switch (attribute)
                {
                    case NativeAttribute.BackgroundColor:
                        view.SetBackgroundColor(ToAndroidColor((System.Drawing.Color)value));
                        break;
                    case NativeAttribute.BorderBottomColor:
                        //view.Border
                        break;
                    case NativeAttribute.BorderBottomEndRadius:
                        break;
                    case NativeAttribute.BorderBottomLeftRadius:
                        break;
                    case NativeAttribute.BorderBottomRightRadius:
                        break;
                    case NativeAttribute.BorderBottomStartRadius:
                        break;
                    case NativeAttribute.BorderBottomWidth:
                        break;
                    case NativeAttribute.BorderColor:
                        break;
                    case NativeAttribute.BorderEndColor:
                        break;
                    case NativeAttribute.BorderLeftColor:
                        break;
                    case NativeAttribute.BorderLeftWidth:
                        break;
                    case NativeAttribute.BorderRadius:
                        break;
                    case NativeAttribute.BorderRightColor:
                        break;
                    case NativeAttribute.BorderRightWidth:
                        break;
                    case NativeAttribute.BorderStartColor:
                        break;
                    case NativeAttribute.BorderStyle:
                        break;
                    case NativeAttribute.BorderTopColor:
                        break;
                    case NativeAttribute.BorderTopEndRadius:
                        break;
                    case NativeAttribute.BorderTopLeftRadius:
                        break;
                    case NativeAttribute.BorderTopRightRadius:
                        break;
                    case NativeAttribute.BorderTopStartRadius:
                        break;
                    case NativeAttribute.BorderTopWidth:
                        break;
                    case NativeAttribute.BorderWidth:
                        break;

                    case NativeAttribute.Opacity:

                        break;
                    case NativeAttribute.Color:
                        var textView = view as TextView;
                        textView.SetTextColor(ToAndroidColor((System.Drawing.Color)value));
                        break;
                    case NativeAttribute.FontFamily:
                        break;
                    case NativeAttribute.FontSize:
                        var texView = view as TextView;
                        texView.TextSize = (float)value;
                        break;
                    case NativeAttribute.FontStyle:
                        break;
                    case NativeAttribute.FontWeight:
                        break;
                    case NativeAttribute.FontVariant:
                        break;
                    case NativeAttribute.LetterSpacing:
                        break;
                    case NativeAttribute.LineHeight:
                        break;
                    case NativeAttribute.TextAlign:
                        break;
                    case NativeAttribute.TextDecorationLine:
                        break;
                    case NativeAttribute.TextShadowColor:
                        break;
                    case NativeAttribute.TextShadowOffsetX:
                        break;
                    case NativeAttribute.TextShadowOffsetY:
                        break;
                    case NativeAttribute.TextShadowRadius:
                        break;
                    case NativeAttribute.TextTransform:
                        break;
                    case NativeAttribute.ResizeMode:
                        break;
                    case NativeAttribute.Source:
                        break;
                    default:
                        return;
                }
            });            
        }

        protected override void SetNodeText(AndroidViewNode node, string text)
        {
            Activity.RunOnUiThread(() =>
            {
                _ = node.AndroidView switch
                {
                    TextView textView => textView.Text = text,
                    _ => throw new NotImplementedException()
                };
            });            
        }

        protected override void UpdateNodeLayout(AndroidViewNode element)
        {
            if(element.Id == 1)
            {
                return;
            }

            Activity.RunOnUiThread(() =>
            {
                var yogaNode = element.FlexNode;

                var view = element.AndroidView;

                if (view is ScrollView scrollView)
                {
                    view = scrollView.GetChildAt(0);
                }

                var layoutParameters = view?.LayoutParameters as RelativeLayout.LayoutParams;

                if (layoutParameters == null)
                {
                    return;
                }

                layoutParameters.LeftMargin = (int)yogaNode.Frame[0];
                layoutParameters.RightMargin = (int)yogaNode.Frame[1];
                layoutParameters.Width = (int)yogaNode.Frame[2];
                layoutParameters.Height = (int)yogaNode.Frame[3];
            });            
        }


        // Element renderers
        View RenderView()
        {
            var view = new RelativeLayout(Activity);
            view.SetBackgroundColor(Color.White);
            return view;
        }

        View RenderText()
        {
            return new TextView(Activity);
        }

        View RenderScrollView()
        {
            var scrollView = new ScrollView(Activity);

            var relativeLayout = new RelativeLayout(Activity);
            var layoutParams = new ScrollView.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

            scrollView.AddView(relativeLayout, layoutParams);

            return scrollView;
        }

        View RenderImage()
        {
            return new ImageView(Activity);
        }

        View RenderButton()
        {
            return new Button(Activity);
        }

        View RenderTextInput()
        {
            return new EditText(Activity);
        }


        static Color ToAndroidColor(System.Drawing.Color color)
        {
            var result = new Color(color.ToArgb());
            return result;
        }


    }
}