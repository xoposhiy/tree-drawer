namespace GraphDrawer;

public class EventsDrawingController
{
    private readonly IStyler styleProvider;
    private readonly NodePositionCalculator positions;
    private readonly IDrawer drawer;

    public EventsDrawingController(NodePositionCalculator positions, IStyler styleProvider, IDrawingSystem drawingSystem)
    {
        this.styleProvider = styleProvider;
        this.positions = positions;
        var imageSize = positions.GetImageSize();
        drawer = drawingSystem.CreateDrawer((int)imageSize.Width, (int)imageSize.Height, styleProvider);
    }

    public void DrawFrames(DrawingEvent[] events)
    {
        drawer.StartFrame();
        foreach (var drawingEvent in events)
        {
            if (drawingEvent is StartNextFrameEvent)
                drawer.StartFrame();
            else
                DrawEvent(drawer, drawingEvent);
        }
    }

    private void DrawEvent(IDrawer context, DrawingEvent drawingEvent)
    {
        switch (drawingEvent)
        {
            case DoNothingEvent:
                break;
            case DrawNodeEvent(var node, var state):
                DrawNode(context, node, state);
                DrawNodeValue(context, node, state);
                foreach (var child in node.RealChildren)
                    DrawArrow(context, node, child, state);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(drawingEvent) + " " + drawingEvent);
        }
    }

    private void DrawArrow(IDrawer context, RealNode parent, RealNode child, NodeState state)
    {
        var start = LowerCenter(positions[parent]);
        var end = UpperCenter(positions[child]);
        var arrowColor = styleProvider.GetArrowColor(state.Type, state.Type);
        var back = (start - end);
        var len = (float)Math.Sqrt(back.X * back.X + back.Y * back.Y);
        back /= len;
        const int arrowSize = 10;
        const int arrowSize2 = 4;
        context.DrawLine(arrowColor, 2, start, end + (arrowSize - 1) * back);
        var side1 = new PointF(-back.Y, back.X);
        var side2 = new PointF(back.Y, -back.X);
        var arrowCap = new[]
            { end, end + arrowSize * back + arrowSize2 * side1, end + arrowSize * back + arrowSize2 * side2 };
        context.FillPolygon(arrowColor, arrowCap);
    }

    private PointF LowerCenter(RectangleF rect)
    {
        return new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height);
    }

    private PointF UpperCenter(RectangleF rect)
    {
        return new PointF(rect.X + rect.Width / 2, rect.Y);
    }

    private void DrawNodeValue(IDrawer context, RealNode node, NodeState state)
    {
        var pos = positions[node];
        var style = styleProvider.GetStyle(state.Type);
        context.DrawText(style.TextStyle, pos, state.Text);
    }

    private void DrawNode(IDrawer context, RealNode node, NodeState state)
    {
        var style = styleProvider.GetStyle(state.Type);
        context.FillRect(style.BackColor, positions[node]);
    }
}