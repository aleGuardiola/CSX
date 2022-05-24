using CSX.Rendering;
using CSX.Skia;
using CSX.Skia.Views;
using SkiaSharp;
using System.Diagnostics;
using Timer = System.Threading.Timer;
using View = CSX.Skia.Views.View;

namespace CSX.Skia.OpenGLTest
{
    public partial class Form1 : Form
    {
        View root;
        Timer RenderTimer;

        ScrollView ScrollView;
        ScrollView ScrollView2;
        ScrollView ScrollView3;

        float totalScroll;

        public Form1()
        {
            InitializeComponent();

            var width = (float)skglControl1.Width;
            var height = (float)skglControl1.Height;

            root = new View(0);
            root.SetAttribute(NativeAttribute.Width, width);
            root.SetAttribute(NativeAttribute.Height, height);
            root.SetAttribute(NativeAttribute.BackgroundColor, Color.Red);            

            ScrollView = GetScrollView();
            ScrollView2 = GetScrollView();
            ScrollView3 = GetScrollView();

            root.AppendView(ScrollView);
            root.AppendView(ScrollView2);
            root.AppendView(ScrollView3);

            ScrollView GetScrollView()
            {

                var totalItems = 10000f;
                var view = new ScrollView(0);
                // view.SetAttribute(NativeAttribute.Width, 512f);
                view.SetAttribute(NativeAttribute.Flex, 1f);
                //view.SetAttribute(NativeAttribute.BackgroundColor, Color.Blue);
                view.SetAttribute(NativeAttribute.BorderColor, Color.Black);
                view.SetAttribute(NativeAttribute.BorderWidth, 1f);
                view.SetAttribute(NativeAttribute.MarginTop, 10f);
                // view.SetAttribute(NativeAttribute.ScrollPosition, totalScroll/*RandomFloat(0, totalScroll)*/);

                Random rnd = new Random();

                for (int i = 0; i < totalItems; i++)
                {
                    var text = new Text(1);
                                        
                    text.SetAttribute(NativeAttribute.BorderColor, Color.Black);
                    text.SetAttribute(NativeAttribute.MarginTop, 10f);                    
                    text.SetAttribute(NativeAttribute.FontWeight, CSX.NativeComponents.FontWeight.Bold);
                    // text.SetAttribute(NativeAttribute.BorderWidth, 1f);
                    text.SetAttribute(NativeAttribute.Width, 200f);
                    text.SetAttribute(NativeAttribute.Height, 13f);

                    text.TextContent = "Hello " + i;

                    view.AppendView(text);
                }

                return view;
            }

            root.CalculateLayout();

            var scrollView = root.Children.First() as View;
            totalScroll = scrollView.GetContentHeight() - (scrollView.YogaNode.LayoutHeight - scrollView.GetBorderTopWidth() - scrollView.GetBorderBottomWidth());

            //RenderTimer = new Timer(OnRender, null, 0, 1000);

        }

        private void OnRender(object? state)
        {
            // root.CalculateLayout();
            //ScrollView.SetAttribute(NativeAttribute.ScrollPosition, RandomFloat(0f, totalScroll));
            ScrollView2.SetAttribute(NativeAttribute.ScrollPosition, RandomFloat(0f, totalScroll));
            //ScrollView3.SetAttribute(NativeAttribute.ScrollPosition, RandomFloat(0f, totalScroll));            
        }

        bool isFirstDraw = true;
        DrawContext context;

        private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
                        
            var sw = new Stopwatch();
            sw.Start();

            if (isFirstDraw)
            {
                context = new DrawContext()
                {
                    ImageInfo = e.Info,
                    SurfaceFactory = () =>
                    {
                        return SKSurface.Create(e.Surface.Context, true, e.Info);
                    }
                };
            }

            if (root.NeedsToReDraw())
            {
                var drawAnything = root.Draw(context.GetCanvas(0), isFirstDraw, 0, null, 0f, context);
                root.MarkAsSeen();
                root.YogaNode.MarkLayoutSeen();

                canvas.Clear(SKColors.White);
                foreach (var surface in context.Surfaces)
                {
                    canvas.DrawSurface(surface, 0, 0);
                }

            }

            sw.Stop();
            var drawTime = sw.ElapsedMilliseconds;

            label1.Text = $"Frame Time: {drawTime}ms,  FPS: {1000.0 / drawTime}";

            isFirstDraw = false;

            skglControl1.Invalidate();
        }


        float RandomFloat(float min, float max)
        {
            Random random = new System.Random();
            double val = (random.NextDouble() * (max - min) + min);
            return (float)val;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ScrollView.SetAttribute(NativeAttribute.ScrollPosition, RandomFloat(0f, totalScroll));
            ScrollView2.SetAttribute(NativeAttribute.ScrollPosition, RandomFloat(0f, totalScroll));
            //ScrollView3.SetAttribute(NativeAttribute.ScrollPosition, RandomFloat(0f, totalScroll));    
        }

        private void skControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            var sw = new Stopwatch();
            sw.Start();

            if (isFirstDraw)
            {
                context = new DrawContext()
                {
                    ImageInfo = e.Info
                };
            }

            if (root.NeedsToReDraw())
            {
                var drawAnything = root.Draw(context.GetCanvas(0), isFirstDraw, 0, null, 0f, context);
                root.MarkAsSeen();
                root.YogaNode.MarkLayoutSeen();

                canvas.Clear(SKColors.White);
                foreach (var surface in context.Surfaces)
                {
                    canvas.DrawSurface(surface, 0, 0);
                }

            }

            sw.Stop();
            var drawTime = sw.ElapsedMilliseconds;

            label1.Text = $"Frame Time: {drawTime}ms,  FPS: {1000.0 / drawTime}";

            isFirstDraw = false;

            //skControl1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            skglControl1.Invalidate();
        }
    }
}