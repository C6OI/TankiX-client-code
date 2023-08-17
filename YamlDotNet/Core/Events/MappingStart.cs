using System.Globalization;

namespace YamlDotNet.Core.Events {
    public class MappingStart : NodeEvent {
        public MappingStart(string anchor, string tag, bool isImplicit, MappingStyle style, Mark start, Mark end)
            : base(anchor, tag, start, end) {
            IsImplicit = isImplicit;
            Style = style;
        }

        public MappingStart(string anchor, string tag, bool isImplicit, MappingStyle style)
            : this(anchor, tag, isImplicit, style, Mark.Empty, Mark.Empty) { }

        public MappingStart()
            : this(null, null, true, MappingStyle.Any, Mark.Empty, Mark.Empty) { }

        public override int NestingIncrease => 1;

        internal override EventType Type => EventType.MappingStart;

        public bool IsImplicit { get; }

        public override bool IsCanonical => !IsImplicit;

        public MappingStyle Style { get; }

        public override string ToString() => string.Format(CultureInfo.InvariantCulture,
            "Mapping start [anchor = {0}, tag = {1}, isImplicit = {2}, style = {3}]",
            Anchor,
            Tag,
            IsImplicit,
            Style);

        public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
    }
}