namespace GraphDrawer;

public class DfsTreeSerializer : AbstractTreeSerializer
{
    protected override IEnumerable<DrawingEvent> GetAnimations(TreeDescription tree, RealNode node, RealNode? parent = null)
    {
        yield return new DrawNodeEvent(node, GetState(node));
        AnimatableNodes.Add(node);
        foreach (var animation in tree.AfterEachNode)
            foreach (var drawingEvent in GetAnimation(animation, parent))
                yield return drawingEvent;
        foreach (var child in node.Children)
        {
            foreach (var animation in GetNodeAnimations(tree, node, child))
                yield return animation;
            foreach (var animation in tree.AfterEachChildSubtree)
                foreach (var drawingEvent in GetAnimation(animation, node))
                    yield return drawingEvent;

        }
        if (node.RealChildren.Any())
            foreach (var animation in tree.AfterLastChild)
            foreach (var drawingEvent in GetAnimation(animation, node))
                yield return drawingEvent;
    }
    
    protected IEnumerable<DrawingEvent> GetNodeAnimations(TreeDescription tree, RealNode parent, Node node)
    {
        switch (node)
        {
            case DrawingEventNode animationChild:
                foreach (var drawingEvent in GetAnimation(animationChild, parent))
                    yield return drawingEvent;
                break;
            case RealNode realChild:
                foreach (var drawingEvent in GetAnimations(tree, realChild, parent))
                    yield return drawingEvent;
                break;
            default:
                throw new InvalidOperationException(node.ToString());
        }
    }

}