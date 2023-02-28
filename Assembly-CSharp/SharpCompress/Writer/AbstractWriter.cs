using System;
using System.IO;
using SharpCompress.Common;

namespace SharpCompress.Writer {
    public abstract class AbstractWriter : IWriter, IDisposable {
        bool closeStream;

        bool isDisposed;

        protected AbstractWriter(ArchiveType type) => WriterType = type;

        protected Stream OutputStream { get; private set; }

        public ArchiveType WriterType { get; }

        public abstract void Write(string filename, Stream source, DateTime? modificationTime);

        public void Dispose() {
            if (!isDisposed) {
                GC.SuppressFinalize(this);
                Dispose(true);
                isDisposed = true;
            }
        }

        protected void InitalizeStream(Stream stream, bool closeStream) {
            OutputStream = stream;
            this.closeStream = closeStream;
        }

        protected virtual void Dispose(bool isDisposing) {
            if (isDisposing && closeStream) {
                OutputStream.Dispose();
            }
        }

        ~AbstractWriter() {
            if (!isDisposed) {
                Dispose(false);
                isDisposed = true;
            }
        }
    }
}