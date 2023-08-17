using System;
using System.Text.RegularExpressions;

namespace YamlDotNet.Core.Events {
    public abstract class NodeEvent : ParsingEvent {
        internal static readonly Regex anchorValidator = new("^[0-9a-zA-Z_\\-]+$", RegexOptions.Compiled);

        protected NodeEvent(string anchor, string tag, Mark start, Mark end)
            : base(start, end) {
            if (anchor != null) {
                if (anchor.Length == 0) {
                    throw new ArgumentException("Anchor value must not be empty.", "anchor");
                }

                if (!anchorValidator.IsMatch(anchor)) {
                    throw new ArgumentException("Anchor value must contain alphanumerical characters only.", "anchor");
                }
            }

            if (tag != null && tag.Length == 0) {
                throw new ArgumentException("Tag value must not be empty.", "tag");
            }

            Anchor = anchor;
            Tag = tag;
        }

        protected NodeEvent(string anchor, string tag)
            : this(anchor, tag, Mark.Empty, Mark.Empty) { }

        public string Anchor { get; }

        public string Tag { get; }

        public abstract bool IsCanonical { get; }
    }
}