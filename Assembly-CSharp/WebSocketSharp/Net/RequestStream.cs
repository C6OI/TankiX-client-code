using System;
using System.IO;

namespace WebSocketSharp.Net {
    internal class RequestStream : Stream {
        long _bodyLeft;

        readonly byte[] _buffer;

        int _count;

        bool _disposed;

        int _offset;

        readonly Stream _stream;

        internal RequestStream(Stream stream, byte[] buffer, int offset, int count)
            : this(stream, buffer, offset, count, -1L) { }

        internal RequestStream(Stream stream, byte[] buffer, int offset, int count, long contentLength) {
            _stream = stream;
            _buffer = buffer;
            _offset = offset;
            _count = count;
            _bodyLeft = contentLength;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        int fillFromBuffer(byte[] buffer, int offset, int count) {
            if (buffer == null) {
                throw new ArgumentNullException("buffer");
            }

            if (offset < 0) {
                throw new ArgumentOutOfRangeException("offset", "A negative value.");
            }

            if (count < 0) {
                throw new ArgumentOutOfRangeException("count", "A negative value.");
            }

            int num = buffer.Length;

            if (offset + count > num) {
                throw new ArgumentException("The sum of 'offset' and 'count' is greater than 'buffer' length.");
            }

            if (_bodyLeft == 0) {
                return -1;
            }

            if (_count == 0 || count == 0) {
                return 0;
            }

            if (count > _count) {
                count = _count;
            }

            if (_bodyLeft > 0 && count > _bodyLeft) {
                count = (int)_bodyLeft;
            }

            Buffer.BlockCopy(_buffer, _offset, buffer, offset, count);
            _offset += count;
            _count -= count;

            if (_bodyLeft > 0) {
                _bodyLeft -= count;
            }

            return count;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            if (_disposed) {
                throw new ObjectDisposedException(GetType().ToString());
            }

            int num = fillFromBuffer(buffer, offset, count);

            if (num > 0 || num == -1) {
                HttpStreamAsyncResult httpStreamAsyncResult = new(callback, state);
                httpStreamAsyncResult.Buffer = buffer;
                httpStreamAsyncResult.Offset = offset;
                httpStreamAsyncResult.Count = count;
                httpStreamAsyncResult.SyncRead = num > 0 ? num : 0;
                httpStreamAsyncResult.Complete();
                return httpStreamAsyncResult;
            }

            if (_bodyLeft >= 0 && count > _bodyLeft) {
                count = (int)_bodyLeft;
            }

            return _stream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) => throw new NotSupportedException();

        public override void Close() {
            _disposed = true;
        }

        public override int EndRead(IAsyncResult asyncResult) {
            if (_disposed) {
                throw new ObjectDisposedException(GetType().ToString());
            }

            if (asyncResult == null) {
                throw new ArgumentNullException("asyncResult");
            }

            if (asyncResult is HttpStreamAsyncResult) {
                HttpStreamAsyncResult httpStreamAsyncResult = (HttpStreamAsyncResult)asyncResult;

                if (!httpStreamAsyncResult.IsCompleted) {
                    httpStreamAsyncResult.AsyncWaitHandle.WaitOne();
                }

                return httpStreamAsyncResult.SyncRead;
            }

            int num = _stream.EndRead(asyncResult);

            if (num > 0 && _bodyLeft > 0) {
                _bodyLeft -= num;
            }

            return num;
        }

        public override void EndWrite(IAsyncResult asyncResult) {
            throw new NotSupportedException();
        }

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count) {
            if (_disposed) {
                throw new ObjectDisposedException(GetType().ToString());
            }

            int num = fillFromBuffer(buffer, offset, count);

            if (num == -1) {
                return 0;
            }

            if (num > 0) {
                return num;
            }

            num = _stream.Read(buffer, offset, count);

            if (num > 0 && _bodyLeft > 0) {
                _bodyLeft -= num;
            }

            return num;
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            throw new NotSupportedException();
        }
    }
}