using System.IO;

namespace SharpCompress.IO {
    internal class NonDisposingStream : Stream {
        public NonDisposingStream(Stream stream) => Stream = stream;

        public Stream Stream { get; }

        public override bool CanRead => Stream.CanRead;

        public override bool CanSeek => Stream.CanSeek;

        public override bool CanWrite => Stream.CanWrite;

        public override long Length => Stream.Length;

        public override long Position {
            get => Stream.Position;
            set => Stream.Position = value;
        }

        protected override void Dispose(bool disposing) {
            if (!disposing) { }
        }

        public override void Flush() {
            Stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count) => Stream.Read(buffer, offset, count);

        public override long Seek(long offset, SeekOrigin origin) => Stream.Seek(offset, origin);

        public override void SetLength(long value) {
            Stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count) {
            Stream.Write(buffer, offset, count);
        }
    }
}