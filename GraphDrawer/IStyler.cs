namespace GraphDrawer
{
    public record NodeStyle(Color BackColor, Color FontColor, string FontName, int FontSize);

    public interface IStyler
    {
        NodeStyle GetStyle(string type);
        Color GetBackColor();
        Color GetArrowColor(string parentType, string childType);
    }
}