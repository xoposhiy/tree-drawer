using System.Collections;

namespace GraphDrawer
{
    public record DrawingEvent;
    public record DoNothingEvent : DrawingEvent;
    public record StartNextFrameEvent : DrawingEvent
    {
        public static readonly StartNextFrameEvent Default = new();
    }
    public record DrawNodeEvent(RealNode Node, NodeState State) : DrawingEvent;
}