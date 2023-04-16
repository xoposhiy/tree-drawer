// See https://aka.ms/new-console-template for more information

using GraphDrawer;

var styler = DefaultStyler.Create();
var generator = new ImagesGenerator(styler);

if (args.Length == 0)
{
    generator.GenerateDirectory("trees");
}
else
{
    foreach (var fileOrDir in args)
    {
        if (Directory.Exists(fileOrDir))
            generator.GenerateDirectory(fileOrDir);
        else if (File.Exists(fileOrDir))
            generator.GenerateFile(fileOrDir);
        else
            Console.WriteLine($"File or directory [{fileOrDir}] not found");
    }
}