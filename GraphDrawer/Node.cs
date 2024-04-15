namespace GraphDrawer;

public class Node
{
}

public abstract class DrawingEventNode : Node
{
}

public class StartFrameEventNode : DrawingEventNode
{
}

public class AnimateParentEventNode : DrawingEventNode
{
}

public class AnimateParentsEventNode : DrawingEventNode
{
}

public class AnimateAllEventNode : DrawingEventNode
{
}

public class AnimateSiblingsEventNode : DrawingEventNode
{
}


public record NodeState(string Text, string Type);

public class RealNode : Node
{
    public RealNode(NodeState[] states, List<Node>? children = null)
    {
        States = states;
        Children = children ?? new List<Node>();
    }
    
    public static implicit operator RealNode(int x) => new(new[]{new NodeState(x.ToString(), "end")});
    public static implicit operator RealNode(string x) => new(new[]{new NodeState(x.ToString(), "end")});
    public IEnumerable<RealNode> RealChildren => Children.OfType<RealNode>();

    public override string ToString() => 
        $"{States[0].Type} {States[0].Text} {Children.Count}";

    public readonly NodeState[] States;
    public readonly List<Node> Children;
}
