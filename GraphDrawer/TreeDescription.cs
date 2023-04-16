namespace GraphDrawer;

public class TreeDescription
{
    public SizeF NodeSize = new(40, 25);
    public SizeF NodeSpacing = new(15, 30);
    public AnimationNode[] AfterEachNode = Array.Empty<AnimationNode>();
    public AnimationNode[] AfterLastChild = Array.Empty<AnimationNode>();
    public RealNode Root = new("root", "a");
}