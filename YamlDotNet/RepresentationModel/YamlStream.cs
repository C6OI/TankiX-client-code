using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace YamlDotNet.RepresentationModel {
    [Serializable]
    public class YamlStream : IEnumerable, IEnumerable<YamlDocument> {
        public YamlStream() { }

        public YamlStream(params YamlDocument[] documents)
            : this((IEnumerable<YamlDocument>)documents) { }

        public YamlStream(IEnumerable<YamlDocument> documents) {
            foreach (YamlDocument document in documents) {
                Documents.Add(document);
            }
        }

        public IList<YamlDocument> Documents { get; } = new List<YamlDocument>();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<YamlDocument> GetEnumerator() => Documents.GetEnumerator();

        public void Add(YamlDocument document) => Documents.Add(document);

        public void Load(TextReader input) => Load(new EventReader(new Parser(input)));

        public void Load(EventReader reader) {
            Documents.Clear();
            reader.Expect<StreamStart>();

            while (!reader.Accept<StreamEnd>()) {
                YamlDocument item = new(reader);
                Documents.Add(item);
            }

            reader.Expect<StreamEnd>();
        }

        public void Save(TextWriter output) => Save(output, true);

        public void Save(TextWriter output, bool assignAnchors) {
            IEmitter emitter = new Emitter(output);
            emitter.Emit(new StreamStart());

            foreach (YamlDocument document in Documents) {
                document.Save(emitter, assignAnchors);
            }

            emitter.Emit(new StreamEnd());
        }

        public void Accept(IYamlVisitor visitor) => visitor.Visit(this);
    }
}