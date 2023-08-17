using System.Globalization;

namespace YamlDotNet.Core.Events {
    public class DocumentEnd : ParsingEvent {
        public DocumentEnd(bool isImplicit, Mark start, Mark end)
            : base(start, end) => IsImplicit = isImplicit;

        public DocumentEnd(bool isImplicit)
            : this(isImplicit, Mark.Empty, Mark.Empty) { }

        public override int NestingIncrease => -1;

        internal override EventType Type => EventType.DocumentEnd;

        public bool IsImplicit { get; }

        public override string ToString() =>
            string.Format(CultureInfo.InvariantCulture, "Document end [isImplicit = {0}]", IsImplicit);

        public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
    }
}