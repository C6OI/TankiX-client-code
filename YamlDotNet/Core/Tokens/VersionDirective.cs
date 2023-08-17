using System;

namespace YamlDotNet.Core.Tokens {
    [Serializable]
    public class VersionDirective : Token {
        public VersionDirective(Version version)
            : this(version, Mark.Empty, Mark.Empty) { }

        public VersionDirective(Version version, Mark start, Mark end)
            : base(start, end) => Version = version;

        public Version Version { get; }

        public override bool Equals(object obj) {
            VersionDirective versionDirective = obj as VersionDirective;
            return versionDirective != null && Version.Equals(versionDirective.Version);
        }

        public override int GetHashCode() => Version.GetHashCode();
    }
}