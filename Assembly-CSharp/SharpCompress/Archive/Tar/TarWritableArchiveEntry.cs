using System;
using System.Collections.Generic;
using System.IO;
using SharpCompress.Common;
using SharpCompress.IO;

namespace SharpCompress.Archive.Tar {
    internal class TarWritableArchiveEntry : TarArchiveEntry {
        readonly bool closeStream;

        internal TarWritableArchiveEntry(TarArchive archive, Stream stream, CompressionType compressionType, string path, long size, DateTime? lastModified, bool closeStream)
            : base(archive, null, compressionType) {
            Stream = stream;
            FilePath = path;
            Size = size;
            LastModifiedTime = lastModified;
            this.closeStream = closeStream;
        }

        public override uint Crc => 0u;

        public override string FilePath { get; }

        public override long CompressedSize => 0L;

        public override long Size { get; }

        public override DateTime? LastModifiedTime { get; }

        public override DateTime? CreatedTime => null;

        public override DateTime? LastAccessedTime => null;

        public override DateTime? ArchivedTime => null;

        public override bool IsEncrypted => false;

        public override bool IsDirectory => false;

        public override bool IsSplit => false;

        internal override IEnumerable<FilePart> Parts => throw new NotImplementedException();

        internal Stream Stream { get; }

        public override Stream OpenEntryStream() => new NonDisposingStream(Stream);

        internal override void Close() {
            if (closeStream) {
                Stream.Dispose();
            }
        }
    }
}