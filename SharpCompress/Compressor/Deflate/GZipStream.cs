using System;
using System.IO;
using SharpCompress.Common;

namespace SharpCompress.Compressor.Deflate {
    public class GZipStream : Stream {
        internal static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        internal ZlibBaseStream BaseStream;

        string comment;

        bool disposed;

        string fileName;

        bool firstReadDone;

        int headerByteCount;

        public GZipStream(Stream stream, CompressionMode mode)
            : this(stream, mode, CompressionLevel.Default, false) { }

        public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level)
            : this(stream, mode, level, false) { }

        public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen)
            : this(stream, mode, CompressionLevel.Default, leaveOpen) { }

        public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen) =>
            BaseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);

        public DateTime? LastModified { get; set; }

        public virtual FlushType FlushMode {
            get => BaseStream._flushMode;
            set {
                if (disposed) {
                    throw new ObjectDisposedException("GZipStream");
                }

                BaseStream._flushMode = value;
            }
        }

        public int BufferSize {
            get => BaseStream._bufferSize;
            set {
                if (disposed) {
                    throw new ObjectDisposedException("GZipStream");
                }

                if (BaseStream._workingBuffer != null) {
                    throw new ZlibException("The working buffer is already set.");
                }

                if (value < 1024) {
                    throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.",
                        value,
                        1024));
                }

                BaseStream._bufferSize = value;
            }
        }

        internal virtual long TotalIn => BaseStream._z.TotalBytesIn;

        internal virtual long TotalOut => BaseStream._z.TotalBytesOut;

        public override bool CanRead {
            get {
                if (disposed) {
                    throw new ObjectDisposedException("GZipStream");
                }

                return BaseStream._stream.CanRead;
            }
        }

        public override bool CanSeek => false;

        public override bool CanWrite {
            get {
                if (disposed) {
                    throw new ObjectDisposedException("GZipStream");
                }

                return BaseStream._stream.CanWrite;
            }
        }

        public override long Length => throw new NotImplementedException();

        public override long Position {
            get {
                if (BaseStream._streamMode == ZlibBaseStream.StreamMode.Writer) {
                    return BaseStream._z.TotalBytesOut + headerByteCount;
                }

                if (BaseStream._streamMode == ZlibBaseStream.StreamMode.Reader) {
                    return BaseStream._z.TotalBytesIn + BaseStream._gzipHeaderByteCount;
                }

                return 0L;
            }
            set => throw new NotImplementedException();
        }

        public string Comment {
            get => comment;
            set {
                if (disposed) {
                    throw new ObjectDisposedException("GZipStream");
                }

                comment = value;
            }
        }

        public string FileName {
            get => fileName;
            set {
                if (disposed) {
                    throw new ObjectDisposedException("GZipStream");
                }

                fileName = value;

                if (fileName != null) {
                    if (fileName.IndexOf("/") != -1) {
                        fileName = fileName.Replace("/", "\\");
                    }

                    if (fileName.EndsWith("\\")) {
                        throw new InvalidOperationException("Illegal filename");
                    }

                    if (fileName.IndexOf("\\") != -1) {
                        fileName = Path.GetFileName(fileName);
                    }
                }
            }
        }

        public int Crc32 { get; private set; }

        protected override void Dispose(bool disposing) {
            try {
                if (!disposed) {
                    if (disposing && BaseStream != null) {
                        BaseStream.Dispose();
                        Crc32 = BaseStream.Crc32;
                    }

                    disposed = true;
                }
            } finally {
                base.Dispose(disposing);
            }
        }

        public override void Flush() {
            if (disposed) {
                throw new ObjectDisposedException("GZipStream");
            }

            BaseStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count) {
            if (disposed) {
                throw new ObjectDisposedException("GZipStream");
            }

            int result = BaseStream.Read(buffer, offset, count);

            if (!firstReadDone) {
                firstReadDone = true;
                FileName = BaseStream._GzipFileName;
                Comment = BaseStream._GzipComment;
            }

            return result;
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();

        public override void SetLength(long value) => throw new NotImplementedException();

        public override void Write(byte[] buffer, int offset, int count) {
            if (disposed) {
                throw new ObjectDisposedException("GZipStream");
            }

            if (BaseStream._streamMode == ZlibBaseStream.StreamMode.Undefined) {
                if (!BaseStream._wantCompress) {
                    throw new InvalidOperationException();
                }

                headerByteCount = EmitHeader();
            }

            BaseStream.Write(buffer, offset, count);
        }

        int EmitHeader() {
            byte[] array = Comment != null ? ArchiveEncoding.Default.GetBytes(Comment) : null;
            byte[] array2 = FileName != null ? ArchiveEncoding.Default.GetBytes(FileName) : null;
            int num = Comment != null ? array.Length + 1 : 0;
            int num2 = FileName != null ? array2.Length + 1 : 0;
            int num3 = 10 + num + num2;
            byte[] array3 = new byte[num3];
            int num4 = 0;
            array3[num4++] = 31;
            array3[num4++] = 139;
            array3[num4++] = 8;
            byte b = 0;

            if (Comment != null) {
                b = (byte)(b ^ 0x10u);
            }

            if (FileName != null) {
                b = (byte)(b ^ 8u);
            }

            array3[num4++] = b;

            if (!LastModified.HasValue) {
                LastModified = DateTime.Now;
            }

            int value = (int)(LastModified.Value - UnixEpoch).TotalSeconds;
            Array.Copy(BitConverter.GetBytes(value), 0, array3, num4, 4);
            num4 += 4;
            array3[num4++] = 0;
            array3[num4++] = byte.MaxValue;

            if (num2 != 0) {
                Array.Copy(array2, 0, array3, num4, num2 - 1);
                num4 += num2 - 1;
                array3[num4++] = 0;
            }

            if (num != 0) {
                Array.Copy(array, 0, array3, num4, num - 1);
                num4 += num - 1;
                array3[num4++] = 0;
            }

            BaseStream._stream.Write(array3, 0, array3.Length);
            return array3.Length;
        }
    }
}