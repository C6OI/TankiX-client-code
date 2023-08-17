using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization.Utilities;

namespace YamlDotNet.Serialization.ValueDeserializers {
    public sealed class AliasValueDeserializer : IValueDeserializer {
        readonly IValueDeserializer innerDeserializer;

        public AliasValueDeserializer(IValueDeserializer innerDeserializer) {
            if (innerDeserializer == null) {
                throw new ArgumentNullException("innerDeserializer");
            }

            this.innerDeserializer = innerDeserializer;
        }

        public object DeserializeValue(EventReader reader, Type expectedType, SerializerState state,
            IValueDeserializer nestedObjectDeserializer) {
            AnchorAlias anchorAlias = reader.Allow<AnchorAlias>();

            if (anchorAlias != null) {
                AliasState aliasState = state.Get<AliasState>();
                ValuePromise value;

                if (!aliasState.TryGetValue(anchorAlias.Value, out value)) {
                    value = new ValuePromise(anchorAlias);
                    aliasState.Add(anchorAlias.Value, value);
                }

                return !value.HasValue ? value : value.Value;
            }

            string text = null;
            NodeEvent nodeEvent = reader.Peek<NodeEvent>();

            if (nodeEvent != null && !string.IsNullOrEmpty(nodeEvent.Anchor)) {
                text = nodeEvent.Anchor;
            }

            object obj = innerDeserializer.DeserializeValue(reader, expectedType, state, nestedObjectDeserializer);

            if (text != null) {
                AliasState aliasState2 = state.Get<AliasState>();
                ValuePromise value2;

                if (!aliasState2.TryGetValue(text, out value2)) {
                    aliasState2.Add(text, new ValuePromise(obj));
                } else {
                    if (value2.HasValue) {
                        throw new DuplicateAnchorException(nodeEvent.Start,
                            nodeEvent.End,
                            string.Format("Anchor '{0}' already defined", text));
                    }

                    value2.Value = obj;
                }
            }

            return obj;
        }

        sealed class AliasState : Dictionary<string, ValuePromise>, IPostDeserializationCallback {
            public void OnDeserialization() {
                foreach (ValuePromise value in Values) {
                    if (!value.HasValue) {
                        throw new AnchorNotFoundException(value.Alias.Start,
                            value.Alias.End,
                            string.Format("Anchor '{0}' not found", value.Alias.Value));
                    }
                }
            }
        }

        sealed class ValuePromise : IValuePromise {
            public readonly AnchorAlias Alias;
            object value;

            public ValuePromise(AnchorAlias alias) => Alias = alias;

            public ValuePromise(object value) {
                HasValue = true;
                this.value = value;
            }

            public bool HasValue { get; private set; }

            public object Value {
                get {
                    if (!HasValue) {
                        throw new InvalidOperationException("Value not set");
                    }

                    return value;
                }
                set {
                    if (HasValue) {
                        throw new InvalidOperationException("Value already set");
                    }

                    HasValue = true;
                    this.value = value;

                    if (ValueAvailable != null) {
                        ValueAvailable(value);
                    }
                }
            }

            public event Action<object> ValueAvailable;
        }
    }
}