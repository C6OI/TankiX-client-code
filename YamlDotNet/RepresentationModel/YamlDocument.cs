using System;
using System.Collections.Generic;
using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel {
    [Serializable]
    public class YamlDocument {
        public YamlDocument(YamlNode rootNode) => RootNode = rootNode;

        public YamlDocument(string rootNode) => RootNode = new YamlScalarNode(rootNode);

        internal YamlDocument(EventReader events) {
            DocumentLoadingState documentLoadingState = new();
            events.Expect<DocumentStart>();

            while (!events.Accept<DocumentEnd>()) {
                RootNode = YamlNode.ParseNode(events, documentLoadingState);

                if (RootNode is YamlAliasNode) {
                    throw new YamlException();
                }
            }

            documentLoadingState.ResolveAliases();
            events.Expect<DocumentEnd>();
        }

        public YamlNode RootNode { get; }

        public IEnumerable<YamlNode> AllNodes => RootNode.AllNodes;

        void AssignAnchors() {
            AnchorAssigningVisitor anchorAssigningVisitor = new();
            anchorAssigningVisitor.AssignAnchors(this);
        }

        internal void Save(IEmitter emitter, bool assignAnchors = true) {
            if (assignAnchors) {
                AssignAnchors();
            }

            emitter.Emit(new DocumentStart());
            RootNode.Save(emitter, new EmitterState());
            emitter.Emit(new DocumentEnd(false));
        }

        public void Accept(IYamlVisitor visitor) => visitor.Visit(this);

        class AnchorAssigningVisitor : YamlVisitor {
            readonly HashSet<string> existingAnchors = new();

            readonly Dictionary<YamlNode, bool> visitedNodes = new();

            public void AssignAnchors(YamlDocument document) {
                existingAnchors.Clear();
                visitedNodes.Clear();
                document.Accept(this);
                Random random = new();

                foreach (KeyValuePair<YamlNode, bool> visitedNode in visitedNodes) {
                    if (visitedNode.Value) {
                        string text;

                        do {
                            text = random.Next().ToString(CultureInfo.InvariantCulture);
                        } while (existingAnchors.Contains(text));

                        existingAnchors.Add(text);
                        visitedNode.Key.Anchor = text;
                    }
                }
            }

            void VisitNode(YamlNode node) {
                if (string.IsNullOrEmpty(node.Anchor)) {
                    bool value;

                    if (visitedNodes.TryGetValue(node, out value)) {
                        if (!value) {
                            visitedNodes[node] = true;
                        }
                    } else {
                        visitedNodes.Add(node, false);
                    }
                } else {
                    existingAnchors.Add(node.Anchor);
                }
            }

            protected override void Visit(YamlScalarNode scalar) => VisitNode(scalar);

            protected override void Visit(YamlMappingNode mapping) => VisitNode(mapping);

            protected override void Visit(YamlSequenceNode sequence) => VisitNode(sequence);
        }
    }
}