using System;
using System.IO;

namespace SharpCompress.IO {
    internal class RewindableStream : Stream {
        MemoryStream bufferStream = new();

        bool isRewound;
        readonly Stream stream;

        public RewindableStream(Stream stream) => this.stream = stream;

        public RewindableStream(Stream stream, MemoryStream bufferStream) {
            RewindableStream rewindableStream = stream as RewindableStream;

            if (rewindableStream != null && !rewindableStream.isRewound) {
                this.stream = rewindableStream.stream;
            } else {
                this.stream = stream;
            }

            this.bufferStream = bufferStream;

            if (bufferStream.Position != bufferStream.Length) {
                isRewound = true;
            }
        }

        internal bool IsRecording { get; private set; }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotImplementedException();

        public override long Position {
            get => stream.Position + bufferStream.Position - bufferStream.Length;
            set {
                if (!isRewound) {
                    stream.Position = value;
                } else if (value < stream.Position - bufferStream.Length || value >= stream.Position) {
                    stream.Position = value;
                    isRewound = false;
                    bufferStream = new MemoryStream();
                } else {
                    bufferStream.Position = value - stream.Position + bufferStream.Length;
                }
            }
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);

            if (disposing) {
                stream.Dispose();
            }
        }

        public void Rewind(bool stopRecording) {
            isRewound = true;
            IsRecording = !stopRecording;
            bufferStream.Position = 0L;
        }

        public void Rewind(MemoryStream buffer) {
            if (bufferStream.Position >= buffer.Length) {
                bufferStream.Position -= buffer.Length;
            } else {
                bufferStream.TransferTo(buffer);
                bufferStream = buffer;
                bufferStream.Position = 0L;
            }

            isRewound = true;
        }

        public void StartRecording() {
            if (bufferStream.Position != 0) {
                byte[] array = bufferStream.ToArray();
                long position = bufferStream.Position;
                bufferStream = new MemoryStream();
                bufferStream.Write(array, (int)position, array.Length - (int)position);
                bufferStream.Position = 0L;
            }

            IsRecording = true;
        }

        public override void Flush() {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count) {
            int num;

            if (isRewound && bufferStream.Position != bufferStream.Length) {
                num = bufferStream.Read(buffer, offset, count);

                if (num < count) {
                    int num2 = stream.Read(buffer, offset + num, count - num);

                    if (IsRecording) {
                        bufferStream.Write(buffer, offset + num, num2);
                    }

                    num += num2;
                }

                if (bufferStream.Position == bufferStream.Length && !IsRecording) {
                    isRewound = false;
                    bufferStream = new MemoryStream();
                }

                return num;
            }

            num = stream.Read(buffer, offset, count);

            if (IsRecording) {
                bufferStream.Write(buffer, offset, num);
            }

            return num;
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        public override void SetLength(long value) {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            throw new NotImplementedException();
        }
    }
}