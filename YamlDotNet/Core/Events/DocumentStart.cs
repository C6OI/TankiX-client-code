using System.Globalization;
using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core.Events {
    public class DocumentStart : ParsingEvent {
        public DocumentStart(VersionDirective version, TagDirectiveCollection tags, bool isImplicit, Mark start, Mark end)
            : base(start, end) {
            Version = version;
            Tags = tags;
            IsImplicit = isImplicit;
        }

        public DocumentStart(VersionDirective version, TagDirectiveCollection tags, bool isImplicit)
            : this(version, tags, isImplicit, Mark.Empty, Mark.Empty) { }

        public DocumentStart(Mark start, Mark end)
            : this(null, null, true, start, end) { }

        public DocumentStart()
            : this(null, null, true, Mark.Empty, Mark.Empty) { }

        public override int NestingIncrease => 1;

        internal override EventType Type => EventType.DocumentStart;

        public TagDirectiveCollection Tags { get; }

        public VersionDirective Version { get; }

        public bool IsImplicit { get; }

        public override string ToString() =>
            string.Format(CultureInfo.InvariantCulture, "Document start [isImplicit = {0}]", IsImplicit);

        public override void Accept(IParsingEventVisitor visitor) => visitor.Visit(this);
    }
}