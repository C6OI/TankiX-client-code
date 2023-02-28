using System;
using System.IO;
using SharpCompress.Archive.Tar;
using SharpCompress.Common;

namespace SharpCompress.Archive {
    public class ArchiveFactory {
        public static IArchive Open(Stream stream, Options options = Options.KeepStreamsOpen) {
            stream.CheckNotNull("stream");

            if (!stream.CanRead || !stream.CanSeek) {
                throw new ArgumentException("Stream should be readable and seekable");
            }

            stream.Seek(0L, SeekOrigin.Begin);

            if (TarArchive.IsTarFile(stream)) {
                stream.Seek(0L, SeekOrigin.Begin);
                return TarArchive.Open(stream, options);
            }

            stream.Seek(0L, SeekOrigin.Begin);
            throw new InvalidOperationException("Cannot determine compressed stream type.");
        }

        public static IArchive Create(ArchiveType type) {
            if (type == ArchiveType.Tar) {
                return TarArchive.Create();
            }

            throw new NotSupportedException("Cannot create Archives of type: " + type);
        }
    }
}