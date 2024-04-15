namespace GraphDrawer;

public class BfsTreeSerializer : AbstractTreeSerializer
{
    protected override IEnumerable<DrawingEvent> GetAnimations(TreeDescription tree, RealNode treeRoot, RealNode? parent)
    {
        var queue = new Queue<(Node node, RealNode? parent)>();
        queue.Enqueue((treeRoot, null));
        while (queue.Any())
        {
            var (node, nodeParent)  = queue.Dequeue();
            if (node is RealNode realNode)
            {
                yield return new DrawNodeEvent(realNode, GetState(realNode));
                AnimatableNodes.Add(realNode);
                foreach (var drawingEventNode in tree.AfterEachNode)
                foreach (var animation in GetAnimation(drawingEventNode, parent))
                    yield return animation;
                foreach (var child in realNode.Children) 
                    queue.Enqueue((child, realNode));
                foreach (var eventNode in tree.AfterLastChild)
                    queue.Enqueue((eventNode, realNode));
            }
            else if (node is DrawingEventNode drawingNode)
                foreach (var drawingEvent in GetAnimation(drawingNode, nodeParent))
                    yield return drawingEvent;
            else
                throw new NotImplementedException(node.GetType().ToString());
        }
    }
}