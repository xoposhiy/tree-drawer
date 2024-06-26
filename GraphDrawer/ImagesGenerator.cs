﻿using GraphDrawer;

public class ImagesGenerator
{
    private readonly IStyler styler;

    public ImagesGenerator(IStyler styler)
    {
        this.styler = styler;
    }

    public void GenerateImage(TreeDescription tree, string outputDirectory)
    {
        if (!Directory.Exists(outputDirectory))
            Directory.CreateDirectory(outputDirectory);
        foreach (var file in Directory.GetFiles(outputDirectory, "*.png"))
            File.Delete(file);
        
        
        ITreeSerializer serializer = tree.TraverseOrder switch
        {
            TraverseOrder.DepthFirst => new DfsTreeSerializer(),
            TraverseOrder.BreadthFirst => new BfsTreeSerializer(),
            _ => throw new ArgumentOutOfRangeException(tree.TraverseOrder.ToString())
        };
        var events = serializer.GetEvents(tree).ToArray();
        //foreach (var ev in events) Console.WriteLine(ev.ToString());

        var drawingSystem = new ImageSharpDrawingSystem();
        var nodePositions = new NodePositionCalculator(tree);
        var drawingController = new EventsDrawingController(nodePositions, styler, drawingSystem);
        drawingController.DrawFrames(events.ToArray());
        
        var i = 0;
        foreach (var image in drawingSystem.Frames)
        {
            var filename =  drawingSystem.Frames.Count > 1 
                    ? $"{outputDirectory}/{i:00}.png" 
                    : $"{outputDirectory}.png";
            i++;
            image.Save(filename);
        }
        Console.WriteLine($"{outputDirectory}\n    {i} images generated!");
    }

    public void GenerateFile(string filename)
    {

        try
        {
            var tree = TreeDescriptionParser.ParseFile(filename);
            GenerateImage(tree, Path.GetFileNameWithoutExtension(filename));
        }
        catch (Exception e)
        {
            throw new Exception($"Error while generating image for {filename}", e);
        }
    }

    public void GenerateDirectory(string directory)
    {
        foreach (var filename in Directory.EnumerateFiles(directory, "*.tree"))
            GenerateFile(filename);
    }

}