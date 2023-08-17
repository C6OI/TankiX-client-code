using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace YamlDotNet.Core.Tokens {
    [Serializable]
    public class TagDirective : Token {
        static readonly Regex tagHandleValidator = new("^!([0-9A-Za-z_\\-]*!)?$", RegexOptions.Compiled);

        public TagDirective(string handle, string prefix)
            : this(handle, prefix, Mark.Empty, Mark.Empty) { }

        public TagDirective(string handle, string prefix, Mark start, Mark end)
            : base(start, end) {
            if (string.IsNullOrEmpty(handle)) {
                throw new ArgumentNullException("handle", "Tag handle must not be empty.");
            }

            if (!tagHandleValidator.IsMatch(handle)) {
                throw new ArgumentException(
                    "Tag handle must start and end with '!' and contain alphanumerical characters only.",
                    "handle");
            }

            Handle = handle;

            if (string.IsNullOrEmpty(prefix)) {
                throw new ArgumentNullException("prefix", "Tag prefix must not be empty.");
            }

            Prefix = prefix;
        }

        public string Handle { get; }

        public string Prefix { get; }

        public override bool Equals(object obj) {
            TagDirective tagDirective = obj as TagDirective;
            return tagDirective != null && Handle.Equals(tagDirective.Handle) && Prefix.Equals(tagDirective.Prefix);
        }

        public override int GetHashCode() => Handle.GetHashCode() ^ Prefix.GetHashCode();

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "{0} => {1}", Handle, Prefix);
    }
}