namespace GraphDrawer;

public class DefaultStyler
{
    public static IStyler Create()
    {
        var fontColor = Color.White;
        var prunedFontColor = Color.DarkGray;
        var prunedNodeColor = Color.LightGray;
        const string fontName = "Consolas";
        const int fontSize = 18;
        var aStyle = new NodeStyle(new Color(new Rgba32(254, 37, 167)), fontColor, fontName, fontSize);
        var bStyle = new NodeStyle(new Color(new Rgba32(26, 179, 213)), fontColor, fontName, fontSize);
        var cStyle = new NodeStyle(new Color(new Rgba32(163, 12, 255)), fontColor, fontName, fontSize);
        var endStyle = new NodeStyle(Color.Black, fontColor, fontName, fontSize);
        var prunedStyle = new NodeStyle(prunedNodeColor, prunedFontColor, fontName, fontSize);
        return new Styler(new ParentColor(), Color.Transparent)
            .AddNodeStyle("a", aStyle)
            .AddNodeStyle("max", aStyle)
            .AddNodeStyle("b", bStyle)
            .AddNodeStyle("min", bStyle)
            .AddNodeStyle("c", cStyle)
            .AddNodeStyle("end", endStyle)
            .AddNodeStyle("pruned", prunedStyle);
    }
}