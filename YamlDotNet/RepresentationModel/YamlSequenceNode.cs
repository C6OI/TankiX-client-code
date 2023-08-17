using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel {
    [Serializable]
    [DebuggerDisplay("Count = {Children.Count}")]
    public class YamlSequenceNode : YamlNode, IEnumerable, IEnumerable<YamlNode> {
        internal YamlSequenceNode(EventReader events, DocumentLoadingState state) {
            SequenceStart yamlEvent = events.Expect<SequenceStart>();
            Load(yamlEvent, state);
            bool flag = false;

            while (!events.Accept<SequenceEnd>()) {
                YamlNode yamlNode = ParseNode(events, state);
                Children.Add(yamlNode);
                flag = flag || yamlNode is YamlAliasNode;
            }

            if (flag) {
                state.AddNodeWithUnresolvedAliases(this);
            }

            events.Expect<SequenceEnd>();
        }

        public YamlSequenceNode() { }

        public YamlSequenceNode(params YamlNode[] children)
            : this((IEnumerable<YamlNode>)children) { }

        public YamlSequenceNode(IEnumerable<YamlNode> children) {
            foreach (YamlNode child in children) {
                Children.Add(child);
            }
        }

        public IList<YamlNode> Children { get; } = new List<YamlNode>();

        public SequenceStyle Style { get; set; }

        public override IEnumerable<YamlNode> AllNodes {
            get {
                yield return this;

                foreach (YamlNode child in Children) {
                    foreach (YamlNode allNode in child.AllNodes) {
                        yield return allNode;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<YamlNode> GetEnumerator() => Children.GetEnumerator();

        public void Add(YamlNode child) => Children.Add(child);

        public void Add(string child) => Children.Add(new YamlScalarNode(child));

        internal override void ResolveAliases(DocumentLoadingState state) {
            for (int i = 0; i < Children.Count; i++) {
                if (Children[i] is YamlAliasNode) {
                    Children[i] = state.GetNode(Children[i].Anchor, true, Children[i].Start, Children[i].End);
                }
            }
        }

        internal override void Emit(IEmitter emitter, EmitterState state) {
            emitter.Emit(new SequenceStart(Anchor, Tag, true, Style));

            foreach (YamlNode child in Children) {
                child.Save(emitter, state);
            }

            emitter.Emit(new SequenceEnd());
        }

        public override void Accept(IYamlVisitor visitor) => visitor.Visit(this);

        public override bool Equals(object other) {
            YamlSequenceNode yamlSequenceNode = other as YamlSequenceNode;

            if (yamlSequenceNode == null || !Equals(yamlSequenceNode) || Children.Count != yamlSequenceNode.Children.Count) {
                return false;
            }

            for (int i = 0; i < Children.Count; i++) {
                if (!SafeEquals(Children[i], yamlSequenceNode.Children[i])) {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode() {
            int num = base.GetHashCode();

            foreach (YamlNode child in Children) {
                num = CombineHashCodes(num, GetHashCode(child));
            }

            return num;
        }

        public override string ToString() {
            StringBuilder stringBuilder = new("[ ");

            foreach (YamlNode child in Children) {
                if (stringBuilder.Length > 2) {
                    stringBuilder.Append(", ");
                }

                stringBuilder.Append(child);
            }

            stringBuilder.Append(" ]");
            return stringBuilder.ToString();
        }
    }
}