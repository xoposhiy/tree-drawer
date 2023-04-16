namespace GraphDrawer;

public class Node
{
}

public abstract class AnimationNode : Node
{
}

public class FrameAnimationNode : AnimationNode
{
}

public class NextValueAnimationNode : AnimationNode
{
}


public class RealNode : Node
{
    public RealNode(string value, string type, List<Node>? children = null)
        :this(new[]{value}, type, children)
    {
    }
    
    public RealNode(string[] values, string type, List<Node>? children = null)
    {
        Values = values;
        Type = type;
        Children = children ?? new List<Node>();
    }

    public static implicit operator RealNode(int x) => new(x.ToString(), "end");
    public static implicit operator RealNode(string x) => new(x, "end");
    public IEnumerable<RealNode> RealChildren => Children.OfType<RealNode>();

    public override string ToString() => 
        $"{Type} {string.Join(" ", Values)} {Children.Count}";

    public readonly string[] Values;
    public readonly string Type;
    public readonly List<Node> Children;
}
