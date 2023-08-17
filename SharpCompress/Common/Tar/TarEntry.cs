using System;
using System.Collections.Generic;
using System.IO;
using SharpCompress.Common.Tar.Headers;
using SharpCompress.IO;

namespace SharpCompress.Common.Tar {
    public class TarEntry : Entry {
        readonly TarFilePart filePart;

        internal TarEntry(TarFilePart filePart, CompressionType type) {
            this.filePart = filePart;
            CompressionType = type;
        }

        public override CompressionType CompressionType { get; }

        public override uint Crc => 0u;

        public override string FilePath => filePart.Header.Name;

        public override long CompressedSize => filePart.Header.Size;

        public override long Size => filePart.Header.Size;

        public override DateTime? LastModifiedTime => filePart.Header.LastModifiedTime;

        public override DateTime? CreatedTime => null;

        public override DateTime? LastAccessedTime => null;

        public override DateTime? ArchivedTime => null;

        public override bool IsEncrypted => false;

        public override bool IsDirectory => filePart.Header.EntryType == EntryType.Directory;

        public override bool IsSplit => false;

        internal override IEnumerable<FilePart> Parts => ((FilePart)filePart).AsEnumerable();

        internal static IEnumerable<TarEntry>
            GetEntries(StreamingMode mode, Stream stream, CompressionType compressionType) {
            foreach (TarHeader h in TarHeaderFactory.ReadHeader(mode, stream)) {
                if (h != null) {
                    if (mode == StreamingMode.Seekable) {
                        yield return new TarEntry(new TarFilePart(h, stream), compressionType);
                    } else {
                        yield return new TarEntry(new TarFilePart(h, null), compressionType);
                    }
                }
            }
        }

        internal override void Close() { }
    }
}