using SkiaSharp;

namespace CSX.Skia.Rendering.Graphic;

public class GraphicContext
{
    SKPictureRecorder? _recorder;
    public SKCanvas? Canvas { get; private set; }

    public GraphicContext()
    {
        
    }

    public void StartPainting(int width, int height)
    {
        _recorder = new SKPictureRecorder();
        Canvas = _recorder.BeginRecording(SKRect.Create(width, height));
    }

    public SKPicture StopPainting()
    {
        if(_recorder == null)
        {
            throw new InvalidOperationException("The painting has not been started");
        }
        
        var picture = _recorder.EndRecording();

        Canvas?.Dispose();
        _recorder?.Dispose();
        Canvas = null;
        _recorder = null;

        var data = picture.Serialize();        

        return picture;
    }

}

