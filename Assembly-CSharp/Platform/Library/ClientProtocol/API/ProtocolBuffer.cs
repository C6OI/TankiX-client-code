using System;
using System.IO;
using Platform.Library.ClientProtocol.Impl;

namespace Platform.Library.ClientProtocol.API {
    public class ProtocolBuffer {
        public ProtocolBuffer()
            : this(new OptionalMap(), new MemoryStreamData()) { }

        public ProtocolBuffer(IOptionalMap optionalMap, MemoryStreamData stream) {
            OptionalMap = optionalMap;
            Data = stream;
        }

        public IOptionalMap OptionalMap { get; }

        public MemoryStreamData Data { get; }

        public BinaryReader Reader => Data.Reader;

        public BinaryWriter Writer => Data.Writer;

        public override string ToString() {
            string text = StreamDumper.Dump(Data.Stream);
            text += Environment.NewLine;
            text += OptionalMap.ToString();
            return text + Environment.NewLine;
        }

        public void Flip() {
            Data.Flip();
        }

        public void Clear() {
            OptionalMap.Clear();
            Data.Clear();
        }
    }
}