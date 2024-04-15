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
        var purple = new Color(new Rgba32(254, 37, 167));
        var violet = new Color(new Rgba32(163, 12, 255));
        var blue = new Color(new Rgba32(26, 179, 213));
        var transparent = Color.Transparent;

        var activeTextStyle = new TextStyle(fontColor, fontName, fontSize);
        var transparentTextStyle = new TextStyle(Color.Transparent, fontName, fontSize);
        var aStyle = new NodeStyle(purple, activeTextStyle, purple);
        var bStyle = new NodeStyle(blue, activeTextStyle, blue);
        var cStyle = new NodeStyle(violet, activeTextStyle, violet);
        var endStyle = new NodeStyle(Color.Black, activeTextStyle, transparent);
        var transparentStyle = new NodeStyle(Color.Transparent, transparentTextStyle, Color.Transparent);
        var prunedStyle = new NodeStyle(prunedNodeColor, new TextStyle(prunedFontColor, fontName, fontSize), prunedNodeColor);
        var unknownStyle = new NodeStyle(prunedNodeColor, new TextStyle(prunedFontColor, fontName, fontSize), transparent);
        return new Styler(new ParentColor(), Color.Transparent)
            .AddNodeStyle("t", transparentStyle)
            .AddNodeStyle("a", aStyle)
            .AddNodeStyle("max", aStyle)
            .AddNodeStyle("b", bStyle)
            .AddNodeStyle("min", bStyle)
            .AddNodeStyle("c", cStyle)
            .AddNodeStyle("n", unknownStyle)
            .AddNodeStyle("end", endStyle)
            .AddNodeStyle("pruned", prunedStyle)
            .AddNodeStyle("p", prunedStyle);
    }
}