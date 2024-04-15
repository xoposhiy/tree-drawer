namespace GraphDrawer;

public interface ITreeSerializer
{
    public IEnumerable<DrawingEvent> GetEvents(TreeDescription tree);
}

public abstract class AbstractTreeSerializer : ITreeSerializer
{
    private readonly Dictionary<RealNode, int> animationIndex = new();
    protected readonly HashSet<RealNode> AnimatableNodes = new();
    
    public IEnumerable<DrawingEvent> GetEvents(TreeDescription tree)
    {
        return GetAnimations(tree, tree.Root, null);
    }

    protected abstract IEnumerable<DrawingEvent> GetAnimations(TreeDescription tree, RealNode treeRoot, RealNode? parent);

    protected void SwitchToNextNodeSate(RealNode node)
    {
        animationIndex[node] = animationIndex.GetValueOrDefault(node, 0) + 1;
    }

    protected NodeState GetState(RealNode node)
    {
        var index = animationIndex.GetValueOrDefault(node, 0);
        if (index >= node.States.Length)
            return node.States.Last();
        return node.States[index];
    }

    protected IEnumerable<DrawingEvent> GetAnimation(DrawingEventNode drawingEventChild, RealNode? parent)
    {
        switch (drawingEventChild)
        {
            case AnimateParentEventNode:
                if (parent is null)
                    yield break;
                SwitchToNextNodeSate(parent);
                yield return new DrawNodeEvent(parent, GetState(parent));
                break;
            case AnimateAllEventNode:
                foreach (var node in AnimatableNodes)
                    SwitchToNextNodeSate(node);
                foreach (var node in AnimatableNodes)
                    yield return new DrawNodeEvent(node, GetState(node));
                break;
            case AnimateSiblingsEventNode:
                if (parent is null) yield break;
                foreach (var sibling in parent.RealChildren)
                {
                    if (!AnimatableNodes.Contains(sibling)) continue;
                    SwitchToNextNodeSate(sibling);
                    yield return new DrawNodeEvent(sibling, GetState(sibling));
                }
                break;
            case StartFrameEventNode:
                yield return new StartNextFrameEvent();
                break;
            default:
                throw new InvalidOperationException(drawingEventChild.ToString());
        }
    }
}