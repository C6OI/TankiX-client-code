using System.Collections.Generic;

namespace YamlDotNet.RepresentationModel {
    public sealed class YamlNodeIdentityEqualityComparer : IEqualityComparer<YamlNode> {
        public bool Equals(YamlNode x, YamlNode y) => ReferenceEquals(x, y);

        public int GetHashCode(YamlNode obj) => obj.GetHashCode();
    }
}