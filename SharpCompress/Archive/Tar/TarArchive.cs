using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common;
using SharpCompress.Common.Tar;
using SharpCompress.Common.Tar.Headers;
using SharpCompress.IO;
using SharpCompress.Reader;
using SharpCompress.Reader.Tar;
using SharpCompress.Writer.Tar;

namespace SharpCompress.Archive.Tar {
    public class TarArchive : AbstractWritableArchive<TarArchiveEntry, TarVolume> {
        internal TarArchive(Stream stream, Options options)
            : base(ArchiveType.Tar, stream.AsEnumerable(), options) { }

        internal TarArchive()
            : base(ArchiveType.Tar) { }

        public static TarArchive Open(Stream stream) {
            stream.CheckNotNull("stream");
            return Open(stream, Options.None);
        }

        public static TarArchive Open(Stream stream, Options options) {
            stream.CheckNotNull("stream");
            return new TarArchive(stream, options);
        }

        public static bool IsTarFile(Stream stream) {
            try {
                TarHeader tarHeader = new();
                tarHeader.Read(new BinaryReader(stream));
                return tarHeader.Name.Length > 0 && Enum.IsDefined(typeof(EntryType), tarHeader.EntryType);
            } catch { }

            return false;
        }

        protected override IEnumerable<TarVolume> LoadVolumes(IEnumerable<Stream> streams, Options options) =>
            new TarVolume(streams.First(), options).AsEnumerable();

        protected override IEnumerable<TarArchiveEntry> LoadEntries(IEnumerable<TarVolume> volumes) {
            Stream stream = volumes.Single().Stream;
            TarHeader previousHeader = null;

            foreach (TarHeader header in TarHeaderFactory.ReadHeader(StreamingMode.Seekable, stream)) {
                if (header == null) {
                    continue;
                }

                if (header.EntryType == EntryType.LongName) {
                    previousHeader = header;
                    continue;
                }

                if (previousHeader != null) {
                    TarArchiveEntry entry = new(this, new TarFilePart(previousHeader, stream), CompressionType.None);
                    MemoryStream memoryStream = new();
                    entry.WriteTo(memoryStream);
                    memoryStream.Position = 0L;
                    byte[] bytes = memoryStream.ToArray();
                    header.Name = ArchiveEncoding.Default.GetString(bytes, 0, bytes.Length).TrimNulls();
                    previousHeader = null;
                }

                yield return new TarArchiveEntry(this, new TarFilePart(header, stream), CompressionType.None);
            }
        }

        public static TarArchive Create() => new();

        protected override TarArchiveEntry CreateEntry(string filePath, Stream source, long size, DateTime? modified,
            bool closeStream) =>
            new TarWritableArchiveEntry(this, source, CompressionType.Unknown, filePath, size, modified, closeStream);

        protected override void SaveTo(Stream stream, CompressionInfo compressionInfo,
            IEnumerable<TarArchiveEntry> oldEntries, IEnumerable<TarArchiveEntry> newEntries) {
            using (TarWriter tarWriter = new(stream, compressionInfo)) {
                foreach (TarArchiveEntry item in from x in oldEntries.Concat(newEntries)
                                                 where !x.IsDirectory
                                                 select x) {
                    using (Stream source = item.OpenEntryStream()) {
                        tarWriter.Write(item.FilePath, source, item.LastModifiedTime, item.Size);
                    }
                }
            }
        }

        protected override IReader CreateReaderForSolidExtraction() {
            Stream stream = Volumes.Single().Stream;
            stream.Position = 0L;
            return TarReader.Open(stream);
        }
    }
}