using System;
using System.IO;

namespace SharpCompress.Compressor.Deflate {
    public class DeflateStream : Stream {
        readonly ZlibBaseStream _baseStream;

        bool _disposed;

        public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level = CompressionLevel.Default,
            bool leaveOpen = false) =>
            _baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);

        public virtual FlushType FlushMode {
            get => _baseStream._flushMode;
            set {
                if (_disposed) {
                    throw new ObjectDisposedException("DeflateStream");
                }

                _baseStream._flushMode = value;
            }
        }

        public int BufferSize {
            get => _baseStream._bufferSize;
            set {
                if (_disposed) {
                    throw new ObjectDisposedException("DeflateStream");
                }

                if (_baseStream._workingBuffer != null) {
                    throw new ZlibException("The working buffer is already set.");
                }

                if (value < 1024) {
                    throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.",
                        value,
                        1024));
                }

                _baseStream._bufferSize = value;
            }
        }

        public CompressionStrategy Strategy {
            get => _baseStream.Strategy;
            set {
                if (_disposed) {
                    throw new ObjectDisposedException("DeflateStream");
                }

                _baseStream.Strategy = value;
            }
        }

        public virtual long TotalIn => _baseStream._z.TotalBytesIn;

        public virtual long TotalOut => _baseStream._z.TotalBytesOut;

        public override bool CanRead {
            get {
                if (_disposed) {
                    throw new ObjectDisposedException("DeflateStream");
                }

                return _baseStream._stream.CanRead;
            }
        }

        public override bool CanSeek => false;

        public override bool CanWrite {
            get {
                if (_disposed) {
                    throw new ObjectDisposedException("DeflateStream");
                }

                return _baseStream._stream.CanWrite;
            }
        }

        public override long Length => throw new NotImplementedException();

        public override long Position {
            get {
                if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Writer) {
                    return _baseStream._z.TotalBytesOut;
                }

                if (_baseStream._streamMode == ZlibBaseStream.StreamMode.Reader) {
                    return _baseStream._z.TotalBytesIn;
                }

                return 0L;
            }
            set => throw new NotImplementedException();
        }

        public MemoryStream InputBuffer =>
            new(_baseStream._z.InputBuffer, _baseStream._z.NextIn, _baseStream._z.AvailableBytesIn);

        protected override void Dispose(bool disposing) {
            try {
                if (!_disposed) {
                    if (disposing && _baseStream != null) {
                        _baseStream.Dispose();
                    }

                    _disposed = true;
                }
            } finally {
                base.Dispose(disposing);
            }
        }

        public override void Flush() {
            if (_disposed) {
                throw new ObjectDisposedException("DeflateStream");
            }

            _baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count) {
            if (_disposed) {
                throw new ObjectDisposedException("DeflateStream");
            }

            return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        public override void SetLength(long value) => throw new NotImplementedException();

        public override void Write(byte[] buffer, int offset, int count) {
            if (_disposed) {
                throw new ObjectDisposedException("DeflateStream");
            }

            _baseStream.Write(buffer, offset, count);
        }
    }
}