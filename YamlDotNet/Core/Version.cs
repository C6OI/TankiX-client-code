using System;

namespace YamlDotNet.Core {
    [Serializable]
    public class Version {
        public Version(int major, int minor) {
            Major = major;
            Minor = minor;
        }

        public int Major { get; }

        public int Minor { get; }

        public override bool Equals(object obj) {
            Version version = obj as Version;
            return version != null && Major == version.Major && Minor == version.Minor;
        }

        public override int GetHashCode() => Major.GetHashCode() ^ Minor.GetHashCode();
    }
}