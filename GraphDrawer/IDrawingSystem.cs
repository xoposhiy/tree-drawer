namespace GraphDrawer;

public interface IDrawingSystem
{
    IDrawer CreateDrawer(int width, int height, IStyler styler);
}

public interface IDrawer
{
    void StartFrame();
    void DrawLine(Color color, int penSize, PointF start, PointF end);
    void FillPolygon(Color fillColor, PointF[] vertices);
    void DrawText(TextStyle style, RectangleF rect, string text);
    void FillRect(Color fillColor, RectangleF rect);
}