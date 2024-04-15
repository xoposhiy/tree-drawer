namespace GraphDrawer;

public class TreeDescription
{
    public SizeF NodeSize = new(40, 25);
    public SizeF NodeSpacing = new(15, 30);
    public SizeF ImageMargins = new(80, 80);
    public TraverseOrder TraverseOrder = TraverseOrder.DepthFirst;
    public DrawingEventNode[] AfterEachNode = Array.Empty<DrawingEventNode>();
    public DrawingEventNode[] AfterEachChildSubtree = Array.Empty<DrawingEventNode>();
    public DrawingEventNode[] AfterLastChild = Array.Empty<DrawingEventNode>();
    public RealNode Root = new(new[]{new NodeState("a", "root")});
}

public enum TraverseOrder
{
    DepthFirst,
    BreadthFirst
}