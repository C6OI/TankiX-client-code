using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.Serialization {
    public sealed class StreamFragment : IYamlSerializable {
        readonly List<ParsingEvent> events = new();

        public IList<ParsingEvent> Events => events;

        void IYamlSerializable.ReadYaml(IParser parser) {
            events.Clear();
            int num = 0;

            do {
                if (!parser.MoveNext()) {
                    throw new InvalidOperationException("The parser has reached the end before deserialization completed.");
                }

                events.Add(parser.Current);
                num += parser.Current.NestingIncrease;
            } while (num > 0);
        }

        void IYamlSerializable.WriteYaml(IEmitter emitter) {
            foreach (ParsingEvent @event in events) {
                emitter.Emit(@event);
            }
        }
    }
}