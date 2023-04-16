using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;

namespace GraphDrawer;

public class TreeDrawer
{
    private readonly TreeDescription tree;
    private readonly IStyler styleProvider;
    private Image<Rgba32> frame;
    private readonly NodePositionCalculator positions;
    private readonly Dictionary<RealNode, int> valueIndex = new();
    public readonly List<Image<Rgba32>> Result = new();
    private readonly SizeF nodeSize;


    public TreeDrawer(TreeDescription tree, IStyler styleProvider)
    {
        this.tree = tree;
        this.styleProvider = styleProvider;
        nodeSize = tree.NodeSize;
        positions = new NodePositionCalculator(tree);
        var imageSize = positions.GetImageSize();
        frame = new Image<Rgba32>((int)imageSize.Width, (int)imageSize.Height, styleProvider.GetBackColor());
    }

    public void DrawFrames()
    {
        Result.Add(frame);
        var animations = GetAnimations(tree.Root);
        foreach (var animation in animations)
        {
            if (animation is StartNextFrame)
            {
                frame = frame.Clone();
                Result.Add(frame);
            }
            else
                frame.Mutate(ctx => DrawAnimation(ctx, animation));
        }
    }

    private IEnumerable<Animation> GetAnimations(RealNode node, RealNode? parent = null)
    {
        yield return new ShowNode(node, GetValue(node));
        foreach (var animation in tree.AfterEachNode)
            yield return GetAnimation(animation, parent);
        foreach (var child in node.Children)
            foreach (var animation in GetNodeAnimations(node, child))
                yield return animation;
        if (node.Children.Any())
            foreach (var animation in tree.AfterLastChild)
                yield return GetAnimation(animation, node);
    }

    private Animation GetAnimation(AnimationNode animationChild, RealNode? parent)
    {
        switch (animationChild)
        {
            case NextValueAnimationNode:
                if (parent == null)
                    throw new InvalidOperationException("Can't animate next-value in root");
                NextValue(parent);
                return new ShowNode(parent, GetValue(parent));
            case FrameAnimationNode:
                return new StartNextFrame();
            default:
                throw new InvalidOperationException(animationChild.ToString());
        }
    }

    private IEnumerable<Animation> GetNodeAnimations(RealNode parent, Node node)
    {
        switch (node)
        {
            case AnimationNode animationChild:
                yield return GetAnimation(animationChild, parent);
                break;
            case RealNode realChild:
                foreach (var animation in GetAnimations(realChild, parent))
                    yield return animation;
                break;
            default:
                throw new InvalidOperationException(node.ToString());
        }
    }

    private void NextValue(RealNode node)
    {
        valueIndex[node] = (valueIndex.TryGetValue(node, out var index) ? index : 0) + 1;
    }

    private string GetValue(RealNode node)
    {
        var index = valueIndex.TryGetValue(node, out var i) ? i : 0;
        if (index >= node.Values.Length)
            throw new InvalidOperationException(node.ToString());
        return node.Values[index];
    }


    private void DrawAnimation(IImageProcessingContext context, Animation animation)
    {
        switch (animation)
        {
            case ShowNode(var node, var nodeValue):
                DrawNode(context, node);
                DrawNodeValue(context, node, nodeValue);
                foreach (var child in node.RealChildren)
                    DrawArrow(context, node, child);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(animation) + " " + animation);
        }
            
    }

    private void DrawArrow(IImageProcessingContext context, RealNode parent, RealNode child)
    {
        var start = positions[parent] + new PointF(0, nodeSize.Height / 2);
        var end = positions[child] + new PointF(0, -nodeSize.Height / 2);
        var arrowColor = styleProvider.GetArrowColor(parent.Type, child.Type);
        var back = (start - end);
        var len = (float)Math.Sqrt(back.X * back.X + back.Y * back.Y);
        back /= len;
        const int arrowSize = 10;
        const int arrowSize2 = 4;
        context.DrawLines(arrowColor, 2, start, end + (arrowSize - 1) * back);
        var side1 = new PointF(-back.Y, back.X);
        var side2 = new PointF(back.Y, -back.X);
        var arrowCap = new[]
            { end, end + arrowSize * back + arrowSize2 * side1, end + arrowSize * back + arrowSize2 * side2 };
        context.FillPolygon(arrowColor, arrowCap);
    }

    private void DrawNodeValue(IImageProcessingContext context, RealNode node, string value)
    {
        var pos = positions[node];
        var style = this.styleProvider.GetStyle(node.Type);
        var fontBrush = new SolidBrush(style.FontColor);
        var textOptions = new TextOptions(SystemFonts.CreateFont(style.FontName, style.FontSize))
        {
            Origin = pos + new PointF(0, 2),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        context.DrawText(textOptions, value, fontBrush);
    }

    private void DrawNode(IImageProcessingContext context, RealNode node)
    {
        var pos = positions[node];
        var style = styleProvider.GetStyle(node.Type);
        var drawingOptions = new DrawingOptions();
        var nodeRect = new RectangleF( pos - nodeSize/2, nodeSize);
        context.Fill(drawingOptions, style.BackColor, nodeRect);
    }
}