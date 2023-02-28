using System;
using System.IO;
using SharpCompress.IO;

namespace SharpCompress.Common {
    public abstract class Volume : IVolume, IDisposable {
        readonly Stream actualStream;

        bool disposed;

        internal Volume(Stream stream, Options options) {
            actualStream = stream;
            Options = options;
        }

        internal Stream Stream => new NonDisposingStream(actualStream);

        internal Options Options { get; }

        public abstract bool IsFirstVolume { get; }

        public abstract bool IsMultiVolume { get; }

        public void Dispose() {
            if (!Options.HasFlag(Options.KeepStreamsOpen) && !disposed) {
                actualStream.Dispose();
                disposed = true;
            }
        }
    }
}