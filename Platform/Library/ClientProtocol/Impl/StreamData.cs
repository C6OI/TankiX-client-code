using System.IO;

namespace Platform.Library.ClientProtocol.Impl {
    public abstract class StreamData {
        bool hasReader;

        bool hasWriter;

        BigEndianBinaryReader reader;

        BigEndianBinaryWriter writer;

        protected StreamData() {
            hasReader = false;
            hasWriter = false;
        }

        public abstract Stream Stream { get; }

        public BigEndianBinaryReader Reader {
            get {
                if (hasReader) {
                    return reader;
                }

                reader = new BigEndianBinaryReader(Stream);
                hasReader = true;
                return reader;
            }
        }

        public BigEndianBinaryWriter Writer {
            get {
                if (hasWriter) {
                    return writer;
                }

                writer = new BigEndianBinaryWriter(Stream);
                hasWriter = true;
                return writer;
            }
        }

        public long Length => Stream.Length;

        public long Position {
            get => Stream.Position;
            set => Stream.Position = value;
        }

        public void SetLength(long value) => Stream.SetLength(value);

        public void Write(byte[] buffer, int offset, int count) => Stream.Write(buffer, offset, count);

        public void WriteByte(byte value) => Stream.WriteByte(value);

        public int Read(byte[] buffer, int offset, int count) => Stream.Read(buffer, offset, count);

        public void Flip() => Stream.Seek(0L, SeekOrigin.Begin);

        public void Clear() {
            Flip();
            SetLength(0L);
        }
    }

    public abstract class StreamData<T> : StreamData where T : Stream, new() {
        protected StreamData() => CastedStream = new T();

        public override Stream Stream => CastedStream;

        public T CastedStream { get; }
    }
}