namespace GraphDrawer;

public class TreeDescriptionParser
{
    public static TreeDescription ParseFile(string filename) => Parse(File.ReadAllLines(filename));

    public static TreeDescription Parse(string[] lines)
    {
        var result = new TreeDescription();
        ParseProperties(result, ref lines);
        var stack = new Stack<RealNode>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            foreach (var (node, depth) in ParseLine(line))
            {
                while (stack.Count > depth)
                    stack.Pop();
                if (stack.Any())
                    stack.Peek().Children.Add(node);
                if (node is RealNode realNode)
                    stack.Push(realNode);
            }
        }

        while (stack.Count > 1)
            stack.Pop();
        result.Root = stack.Pop();
        return result;
    }

    private static void ParseProperties(TreeDescription result, ref string[] lines)
    {
        while (lines.Any() && lines[0].StartsWith("@"))
        {
            var parts = lines[0].Split(" ", 2, StringSplitOptions.RemoveEmptyEntries);
            var key = parts[0];
            var values = (parts.Length > 1 ? parts[1] : "").Split(" ");
            switch (key)
            {
                case "@node-size":
                    result.NodeSize = new SizeF(float.Parse(values[0]), float.Parse(values[1]));
                    break;
                case "@node-spacing":
                    result.NodeSpacing = new SizeF(float.Parse(values[0]), float.Parse(values[1]));
                    break;
                case "@image-margins":
                    result.ImageMargins = new SizeF(float.Parse(values[0]), float.Parse(values[1]));
                    break;
                case "@after-each-node":
                    result.AfterEachNode = values.Select(ParseAnimation).ToArray();
                    break;
                case "@after-last-child":
                    result.AfterLastChild = values.Select(ParseAnimation).ToArray();
                    break;
                case "@after-each-child-subtree":
                    result.AfterEachChildSubtree = values.Select(ParseAnimation).ToArray();
                    break;
                case "@traverse-order":
                    result.TraverseOrder = Enum.Parse<TraverseOrder>(values[0], true);
                    break;
                default:
                    throw new Exception($"Unknown property [{key}]");
            }
            lines = lines[1..];
        }
    }

    public static IEnumerable<(Node node, int depth)> ParseLine(string line)
    {
        var count = line.TakeWhile(char.IsWhiteSpace).Count();
        var depth = line[..count].Count(c => c == ' ') / 4 + line[..count].Count(c => c == '\t');
        var nodeLine = line.TrimStart();
        if (nodeLine.StartsWith("@"))
        {
            var animations = nodeLine.TrimStart('@').Split(" ");
            foreach (var animation in animations)
                yield return (ParseAnimation(animation), depth);
            yield break;
        }

        yield return (ParseRealNode(nodeLine), depth);
    }

    private static RealNode ParseRealNode(string nodeLine)
    {
        var states = nodeLine.Split('|').SelectMany(ParseNodeStates).ToArray();
        return new RealNode(states);
    }

    private static IEnumerable<NodeState> ParseNodeStates(string line)
    {
        var parts = line.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
        var type = parts[0];
        var values = ParseValues(parts.Length > 1 ? parts[1] : "''");
        return values.Select(value => new NodeState(value, type));
    }

    private static DrawingEventNode ParseAnimation(string animation)
    {
        return animation switch
        {
            "frame" => new StartFrameEventNode(),
            "animate-parent" or "next-value" => new AnimateParentEventNode(),
            "animate-siblings" => new AnimateSiblingsEventNode(),
            "animate-all" => new AnimateAllEventNode(),
            _ => throw new Exception($"Unknown animation [{animation}]")
        };
    }

    private static string[] ParseValues(string line)
    {
        var index = 0;
        void SkipSpaces()
        {
            while (index < line.Length && line[index] == ' ')
                index++;
        }
        var values = new List<string>();
        SkipSpaces();
        while (index < line.Length)
        {
            if (line[index] == '\'')
            {
                var startIndex = ++index;
                while (index < line.Length && line[index] != '\'')
                    index++;
                if (index >= line.Length)
                    throw new Exception($"Missing closing quote in [{line}]");
                values.Add(index == startIndex + 1 ? "" : line[startIndex..index]);
                index++;
            }
            else
            {
                var startIndex = index;
                while (index < line.Length && line[index] != ' ')
                    index++;
                values.Add(line[startIndex..index]);
            }
            SkipSpaces();
        }
        return values.ToArray();
    }
}