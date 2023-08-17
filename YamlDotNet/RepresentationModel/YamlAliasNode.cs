using System;
using System.Collections.Generic;
using YamlDotNet.Core;

namespace YamlDotNet.RepresentationModel {
    [Serializable]
    class YamlAliasNode : YamlNode {
        internal YamlAliasNode(string anchor) => Anchor = anchor;

        public override IEnumerable<YamlNode> AllNodes {
            get { yield return this; }
        }

        internal override void ResolveAliases(DocumentLoadingState state) =>
            throw new NotSupportedException("Resolving an alias on an alias node does not make sense");

        internal override void Emit(IEmitter emitter, EmitterState state) =>
            throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be saved.");

        public override void Accept(IYamlVisitor visitor) =>
            throw new NotSupportedException("A YamlAliasNode is an implementation detail and should never be visited.");

        public override bool Equals(object other) {
            YamlAliasNode yamlAliasNode = other as YamlAliasNode;
            return yamlAliasNode != null && Equals(yamlAliasNode) && SafeEquals(Anchor, yamlAliasNode.Anchor);
        }

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => "*" + Anchor;
    }
}