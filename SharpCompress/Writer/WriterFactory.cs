using System;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Writer.Tar;

namespace SharpCompress.Writer {
    public static class WriterFactory {
        public static IWriter Open(Stream stream, ArchiveType archiveType, CompressionType compressionType) => Open(stream,
            archiveType,
            new CompressionInfo {
                Type = compressionType
            });

        public static IWriter Open(Stream stream, ArchiveType archiveType, CompressionInfo compressionInfo) {
            if (archiveType == ArchiveType.Tar) {
                return new TarWriter(stream, compressionInfo);
            }

            throw new NotSupportedException("Archive Type does not have a Writer: " + archiveType);
        }
    }
}