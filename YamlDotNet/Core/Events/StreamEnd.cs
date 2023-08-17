namespace YamlDotNet.Core.Events {
    public class StreamEnd : ParsingEvent {
        public StreamEnd(Mark start, Mark end)
            : base(start, end) { }

        public StreamEnd()
            : this(Mark.Empty, Mark.Empty) { }

        public override int NestingIncrease => -1;

        internal override EventType Type => EventType.StreamEnd;

        public override string ToString() => "Stream end";

        public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
    }
}