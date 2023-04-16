namespace GraphDrawer;

public class ImageSharpDrawingSystem : IDrawingSystem
{
    private readonly List<Image<Rgba32>> frames = new ();
    public IReadOnlyList<Image<Rgba32>> Frames => frames;
    public IDrawer CreateDrawer(int width, int height, IStyler styler)
    {
        return new ImageSharpDrawer(width, height, styler, frames);
    }
}