using System.Globalization;

namespace YamlDotNet.Core.Events {
    public class Scalar : NodeEvent {
        public Scalar(string anchor, string tag, string value, ScalarStyle style, bool isPlainImplicit,
            bool isQuotedImplicit, Mark start, Mark end)
            : base(anchor, tag, start, end) {
            Value = value;
            Style = style;
            IsPlainImplicit = isPlainImplicit;
            IsQuotedImplicit = isQuotedImplicit;
        }

        public Scalar(string anchor, string tag, string value, ScalarStyle style, bool isPlainImplicit,
            bool isQuotedImplicit)
            : this(anchor, tag, value, style, isPlainImplicit, isQuotedImplicit, Mark.Empty, Mark.Empty) { }

        public Scalar(string value)
            : this(null, null, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty) { }

        public Scalar(string tag, string value)
            : this(null, tag, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty) { }

        public Scalar(string anchor, string tag, string value)
            : this(anchor, tag, value, ScalarStyle.Any, true, true, Mark.Empty, Mark.Empty) { }

        internal override EventType Type => EventType.Scalar;

        public string Value { get; }

        public ScalarStyle Style { get; }

        public bool IsPlainImplicit { get; }

        public bool IsQuotedImplicit { get; }

        public override bool IsCanonical => !IsPlainImplicit && !IsQuotedImplicit;

        public override string ToString() => string.Format(CultureInfo.InvariantCulture,
            "Scalar [anchor = {0}, tag = {1}, value = {2}, style = {3}, isPlainImplicit = {4}, isQuotedImplicit = {5}]",
            Anchor,
            Tag,
            Value,
            Style,
            IsPlainImplicit,
            IsQuotedImplicit);

        public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
    }
}