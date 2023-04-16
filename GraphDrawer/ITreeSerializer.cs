namespace GraphDrawer;

public interface ITreeSerializer
{
    public IEnumerable<DrawingEvent> GetEvents(TreeDescription tree);
}

public class DfsTreeSerializer : ITreeSerializer
{
    private readonly Dictionary<RealNode, int> valueIndex = new();
    public IEnumerable<DrawingEvent> GetEvents(TreeDescription tree)
    {
        return GetAnimations(tree, tree.Root);

    }
    private IEnumerable<DrawingEvent> GetAnimations(TreeDescription tree, RealNode node, RealNode? parent = null)
    {
        yield return new DrawNodeEvent(node, GetValue(node));
        foreach (var animation in tree.AfterEachNode)
            yield return GetAnimation(animation, parent);
        foreach (var child in node.Children)
            foreach (var animation in GetNodeAnimations(tree, node, child))
                yield return animation;
        if (node.RealChildren.Any())
            foreach (var animation in tree.AfterLastChild)
                yield return GetAnimation(animation, node);
    }

    private DrawingEvent GetAnimation(DrawingEventNode drawingEventChild, RealNode? parent)
    {
        switch (drawingEventChild)
        {
            case NextValueEventNode:
                if (parent == null)
                    throw new InvalidOperationException("Can't animate next-value in root");
                NextValue(parent);
                return new DrawNodeEvent(parent, GetValue(parent));
            case StartFrameEventNode:
                return new StartNextFrameEvent();
            default:
                throw new InvalidOperationException(drawingEventChild.ToString());
        }
    }

    private IEnumerable<DrawingEvent> GetNodeAnimations(TreeDescription tree, RealNode parent, Node node)
    {
        switch (node)
        {
            case DrawingEventNode animationChild:
                yield return GetAnimation(animationChild, parent);
                break;
            case RealNode realChild:
                foreach (var animation in GetAnimations(tree, realChild, parent))
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

}