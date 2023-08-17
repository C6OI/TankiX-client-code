using System;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Common.Tar.Headers;
using SharpCompress.Compressor;
using SharpCompress.Compressor.BZip2;
using SharpCompress.Compressor.Deflate;

namespace SharpCompress.Writer.Tar {
    public class TarWriter : AbstractWriter {
        public TarWriter(Stream destination, CompressionInfo compressionInfo)
            : base(ArchiveType.Tar) {
            if (!destination.CanWrite) {
                throw new ArgumentException("Tars require writable streams.");
            }

            switch (compressionInfo.Type) {
                case CompressionType.BZip2:
                    destination = new BZip2Stream(destination, CompressionMode.Compress);
                    break;

                case CompressionType.GZip:
                    destination = new GZipStream(destination, CompressionMode.Compress, false);
                    break;

                default:
                    throw new InvalidFormatException("Tar does not support compression: " + compressionInfo.Type);

                case CompressionType.None:
                    break;
            }

            InitalizeStream(destination, false);
        }

        public override void Write(string filename, Stream source, DateTime? modificationTime) =>
            Write(filename, source, modificationTime, null);

        string NormalizeFilename(string filename) {
            filename = filename.Replace('\\', '/');
            int num = filename.IndexOf(':');

            if (num >= 0) {
                filename = filename.Remove(0, num + 1);
            }

            return filename.Trim('/');
        }

        public void Write(string filename, Stream source, DateTime? modificationTime, long? size) {
            if (!source.CanSeek && !size.HasValue) {
                throw new ArgumentException("Seekable stream is required if no size is given.");
            }

            long size2 = !size.HasValue ? source.Length : size.Value;
            TarHeader tarHeader = new();
            tarHeader.LastModifiedTime = !modificationTime.HasValue ? TarHeader.Epoch : modificationTime.Value;
            tarHeader.Name = NormalizeFilename(filename);
            tarHeader.Size = size2;
            tarHeader.Write(OutputStream);
            size = source.TransferTo(OutputStream);
            PadTo512(size.Value, false);
        }

        void PadTo512(long size, bool forceZeros) {
            int num = (int)size % 512;

            if (num != 0 || forceZeros) {
                num = 512 - num;
                OutputStream.Write(new byte[num], 0, num);
            }
        }

        protected override void Dispose(bool isDisposing) {
            if (isDisposing) {
                PadTo512(0L, true);
                PadTo512(0L, true);
                OutputStream.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}