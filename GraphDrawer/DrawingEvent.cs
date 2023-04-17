namespace GraphDrawer
{
    public record DrawingEvent;

    public record DoNothingEvent : DrawingEvent;
    public record StartNextFrameEvent : DrawingEvent
    {
        public static readonly StartNextFrameEvent Default = new();
    }
    public record DrawNodeEvent(RealNode Node, string NodeValue) : DrawingEvent;
}