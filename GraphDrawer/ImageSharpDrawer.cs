using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

namespace GraphDrawer;

public class ImageSharpDrawer : IDrawer
{
    private readonly int width;
    private readonly int height;
    private readonly IStyler styler;
    private readonly List<Image<Rgba32>> frames;

    public Image<Rgba32> CurrentFrame => frames[^1];

    public ImageSharpDrawer(int width, int height, IStyler styler, List<Image<Rgba32>> frames)
    {
        this.width = width;
        this.height = height;
        this.styler = styler;
        this.frames = frames;
    }

    public void StartFrame()
    {
        var newFrame = frames.Count == 0 
            ? new Image<Rgba32>(width, height, styler.GetBackColor()) 
            : CurrentFrame.Clone();
        frames.Add(newFrame);
    }

    public void DrawLine(Color color, int penSize, PointF start, PointF end)
    {
        CurrentFrame.Mutate(ctx => ctx.DrawLines(new Pen(color, penSize), start, end));
    }

    public void FillPolygon(Color fillColor, PointF[] vertices)
    {
        CurrentFrame.Mutate(ctx => ctx.FillPolygon(fillColor, vertices));
    }

    public void DrawText(TextStyle style, RectangleF rect, string text)
    {
        CurrentFrame.Mutate(ctx =>
        {
            var pos = rect.Location + new SizeF(rect.Width / 2, rect.Height / 2 + 2);
            var fontBrush = new SolidBrush(style.FontColor);
            var textOptions = new TextOptions(SystemFonts.CreateFont(style.FontName, style.FontSize))
            {
                Origin = pos,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            ctx.DrawText(textOptions, text, fontBrush);
        });
    }

    public void FillRect(Color fillColor, RectangleF rect)
    {
        CurrentFrame.Mutate(ctx => ctx.Fill(fillColor, rect));
    }
}