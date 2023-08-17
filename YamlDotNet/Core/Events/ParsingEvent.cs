namespace YamlDotNet.Core.Events {
    public abstract class ParsingEvent {
        internal ParsingEvent(Mark start, Mark end) {
            Start = start;
            End = end;
        }

        public virtual int NestingIncrease => 0;

        internal abstract EventType Type { get; }

        public Mark Start { get; }

        public Mark End { get; }

        public abstract void Accept(IParsingEventVisitor visitor);
    }
}