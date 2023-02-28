using System;
using System.IO;

namespace SharpCompress.Compressor.Deflate {
    public class ZlibStream : Stream {
        readonly ZlibBaseStream _baseStream;

        bool _disposed;

        public ZlibStream(Stream stream, CompressionMode mode)
            : this(stream, mode, CompressionLevel.Default, false) { }

        public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level)
            : this(stream, mode, level, false) { }

        public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen)
            : this(stream, mode, CompressionLevel.Default, leaveOpen) { }

        public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen) =>
            _baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);

        public virtual FlushType FlushMode {
            get => _baseStream._flushMode;
            set {
                if (_disposed) {
                    throw new ObjectDisposedException("ZlibStream");
                }

                _baseStream._flushMode = value;
            }
        }

        public int BufferSize {
            get => _baseStream._bufferSize;
            set {
                if (_disposed) {
                    throw new ObjectDisposedException("ZlibStream");
                }

                if (_baseStream._workingBuffer != null) {
                    throw new ZlibException("The working buffer is already set.");
                }

                if (value < 1024) {
                    throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
                }

                _baseStream._bufferSize = value;
            }
        }

        public virtual long TotalIn => _baseStream._z.TotalBytesIn;

        public virtual long TotalOut => _baseStream._z.TotalBytesOut;

        public override bool CanRead {
            get {
                if (_disposed) {
                    throw new ObjectDisposedException("ZlibStream");
                }

                return _baseStream._stream.CanRead;
            }
        }

        public override bool CanSeek => false;

        public override bool CanWrite {
            get {
                if (_disposed) {
                    throw new ObjectDisposedException("ZlibStream");
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
                throw new ObjectDisposedException("ZlibStream");
            }

            _baseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count) {
            if (_disposed) {
                throw new ObjectDisposedException("ZlibStream");
            }

            return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        public override void SetLength(long value) {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            if (_disposed) {
                throw new ObjectDisposedException("ZlibStream");
            }

            _baseStream.Write(buffer, offset, count);
        }
    }
}