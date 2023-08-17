namespace YamlDotNet.Core.Events {
    public class MappingEnd : ParsingEvent {
        public MappingEnd(Mark start, Mark end)
            : base(start, end) { }

        public MappingEnd()
            : this(Mark.Empty, Mark.Empty) { }

        public override int NestingIncrease => -1;

        internal override EventType Type => EventType.MappingEnd;

        public override string ToString() => "Mapping end";

        public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
    }
}