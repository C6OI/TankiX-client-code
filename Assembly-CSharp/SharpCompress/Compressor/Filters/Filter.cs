using System;
using System.IO;

namespace SharpCompress.Compressor.Filters {
    public abstract class Filter : Stream {
        protected Stream baseStream;

        bool endReached;
        protected bool isEncoder;

        int read;

        readonly byte[] tail;

        int transformed;

        readonly byte[] window;

        protected Filter(bool isEncoder, Stream baseStream, int lookahead) {
            this.isEncoder = isEncoder;
            this.baseStream = baseStream;
            tail = new byte[lookahead - 1];
            window = new byte[tail.Length * 2];
        }

        public override bool CanRead => !isEncoder;

        public override bool CanSeek => false;

        public override bool CanWrite => isEncoder;

        public override long Length => baseStream.Length;

        public override long Position {
            get => baseStream.Position;
            set => throw new NotImplementedException();
        }

        public override void Flush() {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count) {
            int num = 0;

            if (transformed > 0) {
                int num2 = transformed;

                if (num2 > count) {
                    num2 = count;
                }

                Buffer.BlockCopy(tail, 0, buffer, offset, num2);
                transformed -= num2;
                read -= num2;
                offset += num2;
                count -= num2;
                num += num2;
                Buffer.BlockCopy(tail, num2, tail, 0, read);
            }

            if (count == 0) {
                return num;
            }

            int num3 = read;

            if (num3 > count) {
                num3 = count;
            }

            Buffer.BlockCopy(tail, 0, buffer, offset, num3);
            read -= num3;
            Buffer.BlockCopy(tail, num3, tail, 0, read);

            while (!endReached && num3 < count) {
                int num4 = baseStream.Read(buffer, offset + num3, count - num3);
                num3 += num4;

                if (num4 == 0) {
                    endReached = true;
                }
            }

            while (!endReached && read < tail.Length) {
                int num5 = baseStream.Read(tail, read, tail.Length - read);
                read += num5;

                if (num5 == 0) {
                    endReached = true;
                }
            }

            if (num3 > tail.Length) {
                transformed = Transform(buffer, offset, num3);
                offset += transformed;
                count -= transformed;
                num += transformed;
                num3 -= transformed;
                transformed = 0;
            }

            if (count == 0) {
                return num;
            }

            Buffer.BlockCopy(buffer, offset, window, 0, num3);
            Buffer.BlockCopy(tail, 0, window, num3, read);

            if (num3 + read > tail.Length) {
                transformed = Transform(window, 0, num3 + read);
            } else {
                transformed = num3 + read;
            }

            Buffer.BlockCopy(window, 0, buffer, offset, num3);
            Buffer.BlockCopy(window, num3, tail, 0, read);
            num += num3;
            transformed -= num3;
            return num;
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        public override void SetLength(long value) {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count) {
            Transform(buffer, offset, count);
            baseStream.Write(buffer, offset, count);
        }

        protected abstract int Transform(byte[] buffer, int offset, int count);
    }
}