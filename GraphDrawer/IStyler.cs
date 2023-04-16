namespace GraphDrawer
{
    public record TextStyle(Color FontColor, string FontName, int FontSize);
    public record NodeStyle(Color BackColor, TextStyle TextStyle);

    public interface IStyler
    {
        NodeStyle GetStyle(string type);
        Color GetBackColor();
        Color GetArrowColor(string parentType, string childType);
    }
}