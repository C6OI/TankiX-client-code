using System.Globalization;

namespace YamlDotNet.Core.Events {
    public class AnchorAlias : ParsingEvent {
        public AnchorAlias(string value, Mark start, Mark end)
            : base(start, end) {
            if (string.IsNullOrEmpty(value)) {
                throw new YamlException(start, end, "Anchor value must not be empty.");
            }

            if (!NodeEvent.anchorValidator.IsMatch(value)) {
                throw new YamlException(start, end, "Anchor value must contain alphanumerical characters only.");
            }

            Value = value;
        }

        public AnchorAlias(string value)
            : this(value, Mark.Empty, Mark.Empty) { }

        internal override EventType Type => EventType.Alias;

        public string Value { get; }

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Alias [value = {0}]", Value);

        public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
    }
}