// See https://aka.ms/new-console-template for more information
// CREATING THE DRAWING SURFACE

// constract the information describing the desired surface
using CSX.NativeComponents;
using CSX.Rendering;
using CSX.Skia.Views;
using SkiaSharp;
using System.Drawing;
using ScrollView = CSX.Skia.Views.ScrollView;
using Text = CSX.Skia.Views.Text;
using View = CSX.Skia.Views.View;

var info = new SKImageInfo(512, 512);
// create the surface using the information
var surface = SKSurface.Create(info);


// CLEARING THE SURFACE

// get the canvas that we can draw on
var canvas = surface.Canvas;

var root = new View(0);
root.SetAttribute(NativeAttribute.Width, 512f);
root.SetAttribute(NativeAttribute.Height, 512f);
root.SetAttribute(NativeAttribute.BackgroundColor, Color.White);

root.AppendView(GetScrollView());
root.AppendView(GetScrollView());
root.AppendView(GetScrollView());

root.CalculateLayout();
root.Draw(canvas);

// create a raster instance of the surface
var image = surface.Snapshot();
// encode the image as a PNG
var data = image.Encode(SKEncodedImageFormat.Jpeg, 100);
// save the PNG to disk
using (var stream = File.OpenWrite("output.jpeg"))
{
    // copy the encoded image into the file stream
    data.SaveTo(stream);
}

static ScrollView GetScrollView()
{

    var totalItems = 10000f;
    var totalScroll = (totalItems * 13f) - (170f - 20f);

    var view = new ScrollView(0);
    view.SetAttribute(NativeAttribute.Width, 512f);
    view.SetAttribute(NativeAttribute.Height, 170f);
    view.SetAttribute(NativeAttribute.BackgroundColor, Color.Red);
    view.SetAttribute(NativeAttribute.BorderColor, Color.Black);
    view.SetAttribute(NativeAttribute.BorderWidth, 10f);
    view.SetAttribute(NativeAttribute.ScrollPosition, totalScroll/*RandomFloat(0, totalScroll)*/);

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

static float RandomFloat(float min, float max)
{
    Random random = new System.Random();
    double val = (random.NextDouble() * (max - min) + min);
    return (float)val;
}