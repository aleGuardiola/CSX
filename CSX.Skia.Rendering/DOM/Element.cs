using Facebook.Yoga;

namespace CSX.Skia.Rendering.DOM;

public abstract class Element
{
    public YogaNode YogaNode { get; private set; }

    public Transform Transform { get; private set; }

    public Element()
    {
        YogaNode = new YogaNode();
        Transform = new Transform();
    } 

    public void 

}
