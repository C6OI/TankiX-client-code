namespace YamlDotNet.Core.Events {
    public class SequenceEnd : ParsingEvent {
        public SequenceEnd(Mark start, Mark end)
            : base(start, end) { }

        public SequenceEnd()
            : this(Mark.Empty, Mark.Empty) { }

        public override int NestingIncrease => -1;

        internal override EventType Type => EventType.SequenceEnd;

        public override string ToString() => "Sequence end";

        public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
    }
}