using GraphDrawer;

public class ImagesGenerator
{
    private readonly IStyler styler;

    public ImagesGenerator(IStyler styler)
    {
        this.styler = styler;
    }

    public void GenerateImage(string outputDirectory)
    {
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);
        foreach (var file in Directory.GetFiles(outputDirectory, "*.png"))
            File.Delete(file);
        var tree = TreeDescriptionParser.ParseFile($"trees/{outputDirectory}.tree");
        var drawer = new TreeDrawer(tree, styler);
        drawer.DrawFrames();
        var i = 0;
        foreach (var image in drawer.Result)
            image.Save($"{outputDirectory}/{i++:00}.png");
        Console.WriteLine($"{outputDirectory}\n    {i} images generated!");
    }

    public void GenerateFile(string filename)
    {
        GenerateImage(Path.GetFileNameWithoutExtension(filename));
    }

    public void GenerateDirectory(string directory)
    {
        foreach (var filename in Directory.EnumerateFiles(directory, "*.tree"))
            GenerateFile(filename);
    }

}