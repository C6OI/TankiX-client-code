using System;
using System.IO;
using System.Text;

namespace WebSocketSharp.Net {
    internal class ResponseStream : Stream {
        static readonly byte[] _crlf = new byte[2] { 13, 10 };
        MemoryStream _body;

        bool _disposed;

        HttpListenerResponse _response;

        bool _sendChunked;

        Stream _stream;

        readonly Action<byte[], int, int> _write;

        Action<byte[], int, int> _writeBody;

        readonly Action<byte[], int, int> _writeChunked;

        internal ResponseStream(Stream stream, HttpListenerResponse response, bool ignoreWriteExceptions) {
            _stream = stream;
            _response = response;

            if (ignoreWriteExceptions) {
                _write = writeWithoutThrowingException;
                _writeChunked = writeChunkedWithoutThrowingException;
            } else {
                _write = stream.Write;
                _writeChunked = writeChunked;
            }

            _body = new MemoryStream();
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => !_disposed;

        public override long Length => throw new NotSupportedException();

        public override long Position {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        bool flush(bool closing) {
            if (!_response.HeadersSent) {
                if (!flushHeaders(closing)) {
                    if (closing) {
                        _response.CloseConnection = true;
                    }

                    return false;
                }

                _sendChunked = _response.SendChunked;
                _writeBody = !_sendChunked ? _write : _writeChunked;
            }

            flushBody(closing);

            if (closing && _sendChunked) {
                byte[] chunkSizeBytes = getChunkSizeBytes(0, true);
                _write(chunkSizeBytes, 0, chunkSizeBytes.Length);
            }

            return true;
        }

        void flushBody(bool closing) {
            using (_body) {
                long length = _body.Length;

                if (length > int.MaxValue) {
                    _body.Position = 0L;
                    int num = 1024;
                    byte[] array = new byte[num];
                    int num2 = 0;

                    while ((num2 = _body.Read(array, 0, num)) > 0) {
                        _writeBody(array, 0, num2);
                    }
                } else if (length > 0) {
                    _writeBody(_body.GetBuffer(), 0, (int)length);
                }
            }

            _body = closing ? null : new MemoryStream();
        }

        bool flushHeaders(bool closing) {
            using (MemoryStream memoryStream = new()) {
                WebHeaderCollection webHeaderCollection = _response.WriteHeadersTo(memoryStream);
                long position = memoryStream.Position;
                long num = memoryStream.Length - position;

                if (num > 32768) {
                    return false;
                }

                if (!_response.SendChunked && _response.ContentLength64 != _body.Length) {
                    return false;
                }

                _write(memoryStream.GetBuffer(), (int)position, (int)num);
                _response.CloseConnection = webHeaderCollection["Connection"] == "close";
                _response.HeadersSent = true;
            }

            return true;
        }

        static byte[] getChunkSizeBytes(int size, bool final) => Encoding.ASCII.GetBytes(string.Format("{0:x}\r\n{1}", size, !final ? string.Empty : "\r\n"));

        void writeChunked(byte[] buffer, int offset, int count) {
            byte[] chunkSizeBytes = getChunkSizeBytes(count, false);
            _stream.Write(chunkSizeBytes, 0, chunkSizeBytes.Length);
            _stream.Write(buffer, offset, count);
            _stream.Write(_crlf, 0, 2);
        }

        void writeChunkedWithoutThrowingException(byte[] buffer, int offset, int count) {
            try {
                writeChunked(buffer, offset, count);
            } catch { }
        }

        void writeWithoutThrowingException(byte[] buffer, int offset, int count) {
            try {
                _stream.Write(buffer, offset, count);
            } catch { }
        }

        internal void Close(bool force) {
            if (_disposed) {
                return;
            }

            _disposed = true;

            if (!force && flush(true)) {
                _response.Close();
            } else {
                if (_sendChunked) {
                    byte[] chunkSizeBytes = getChunkSizeBytes(0, true);
                    _write(chunkSizeBytes, 0, chunkSizeBytes.Length);
                }

                _body.Dispose();
                _body = null;
                _response.Abort();
            }

            _response = null;
            _stream = null;
        }

        internal void InternalWrite(byte[] buffer, int offset, int count) {
            _write(buffer, offset, count);
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) => throw new NotSupportedException();

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) {
            if (_disposed) {
                throw new ObjectDisposedException(GetType().ToString());
            }

            return _body.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close() {
            Close(false);
        }

        protected override void Dispose(bool disposing) {
            Close(!disposing);
        }

        public override int EndRead(IAsyncResult asyncResult) => throw new NotSupportedException();

        public override void EndWrite(IAsyncResult asyncResult) {
            if (_disposed) {
                throw new ObjectDisposedException(GetType().ToString());
            }

            _body.EndWrite(asyncResult);
        }

        public override void Flush() {
            if (!_disposed && (_sendChunked || _response.SendChunked)) {
                flush(false);
            }
        }

        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            if (_disposed) {
                throw new ObjectDisposedException(GetType().ToString());
            }

            _body.Write(buffer, offset, count);
        }
    }
}