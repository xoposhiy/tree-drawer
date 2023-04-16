using System.Runtime.CompilerServices;

namespace GraphDrawer
{
    public record Animation;

    public record StartNextFrame : Animation
    {
        public static readonly StartNextFrame Default = new();
    }
    public record ShowNode(RealNode Node, string NodeValue) : Animation;
}