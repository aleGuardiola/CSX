using CSX.Rendering;
using CSX.Skia;
using CSX.Skia.Views;
using Timer = System.Threading.Timer;
using View = CSX.Skia.Views.View;

namespace CSX.Skia.OpenGLTest
{
    public partial class Form1 : Form
    {
        View root;
        Timer RenderTimer;

        public Form1()
        {
            InitializeComponent();

            root = new View(0);
            root.SetAttribute(NativeAttribute.Width, 802f);
            root.SetAttribute(NativeAttribute.Height, 451f);
            root.SetAttribute(NativeAttribute.BackgroundColor, Color.White);

            root.AppendView(GetScrollView());
            root.AppendView(GetScrollView());
            root.AppendView(GetScrollView());

            ScrollView GetScrollView()
            {

                var totalItems = 10000f;
                var totalScroll = (totalItems * 13f) - (170f - 20f);

                var view = new ScrollView(0);
                // view.SetAttribute(NativeAttribute.Width, 512f);
                view.SetAttribute(NativeAttribute.Height, 451f / 3f);
                view.SetAttribute(NativeAttribute.BackgroundColor, Color.Red);
                view.SetAttribute(NativeAttribute.BorderColor, Color.Black);
                view.SetAttribute(NativeAttribute.BorderWidth, 10f);
                // view.SetAttribute(NativeAttribute.ScrollPosition, totalScroll/*RandomFloat(0, totalScroll)*/);

                Random rnd = new Random();

                for (int i = 0; i < totalItems; i++)
                {
                    Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

                    var text = new View(1);

                    text.SetAttribute(NativeAttribute.BackgroundColor, randomColor);
                    text.SetAttribute(NativeAttribute.BorderColor, Color.Black);
                    // text.SetAttribute(NativeAttribute.BorderWidth, 1f);
                    text.SetAttribute(NativeAttribute.Width, 200f);
                    text.SetAttribute(NativeAttribute.Height, 13f);

                    view.AppendView(text);
                }

                return view;
            }

            float RandomFloat(float min, float max)
            {
                Random random = new System.Random();
                double val = (random.NextDouble() * (max - min) + min);
                return (float)val;
            }

            root.CalculateLayout();
            RenderTimer = new Timer(OnRender, null, 0, 1000);

        }

        private void OnRender(object? state)
        {
           // root.CalculateLayout();
            
            skglControl.Invalidate();
        }

        private void skglControl1_PaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;

            canvas.Clear();

            root.Draw(canvas);
        }

    }
}