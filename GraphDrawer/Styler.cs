namespace GraphDrawer
{
    public record ArrowColor;
    public record ArrowConstantColor(Color Color) : ArrowColor;
    public record ParentColor : ArrowColor;
    public record ChildColor : ArrowColor;

    public class Styler : IStyler
    {
        private readonly ArrowColor arrowColor;
        private readonly Color backColor;
        private readonly Dictionary<string, NodeStyle> nodeNodeStyles = new Dictionary<string, NodeStyle>();

        public Styler(ArrowColor arrowColor, Color backColor)
        {
            this.arrowColor = arrowColor;
            this.backColor = backColor;
        }

        public IReadOnlyDictionary<string, NodeStyle> NodeStyles => nodeNodeStyles;

        public Styler AddNodeStyle(string name, NodeStyle style)
        {
            nodeNodeStyles[name] = style;
            return this;
        }
        
        public NodeStyle GetStyle(string type)
        {
            return nodeNodeStyles[type];
        }

        public Color GetBackColor() => backColor;

        public Color GetArrowColor(string parentType, string childType) =>
            arrowColor switch
            {
                ArrowConstantColor(var color) => color,
                ChildColor => GetStyle(childType).BackColor,
                ParentColor => GetStyle(parentType).OutgoingArrowColor,
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}