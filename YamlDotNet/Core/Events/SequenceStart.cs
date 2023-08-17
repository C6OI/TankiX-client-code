using System.Globalization;

namespace YamlDotNet.Core.Events {
    public class SequenceStart : NodeEvent {
        public SequenceStart(string anchor, string tag, bool isImplicit, SequenceStyle style, Mark start, Mark end)
            : base(anchor, tag, start, end) {
            IsImplicit = isImplicit;
            Style = style;
        }

        public SequenceStart(string anchor, string tag, bool isImplicit, SequenceStyle style)
            : this(anchor, tag, isImplicit, style, Mark.Empty, Mark.Empty) { }

        public override int NestingIncrease => 1;

        internal override EventType Type => EventType.SequenceStart;

        public bool IsImplicit { get; }

        public override bool IsCanonical => !IsImplicit;

        public SequenceStyle Style { get; }

        public override string ToString() => string.Format(CultureInfo.InvariantCulture,
            "Sequence start [anchor = {0}, tag = {1}, isImplicit = {2}, style = {3}]",
            Anchor,
            Tag,
            IsImplicit,
            Style);

        public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
    }
}