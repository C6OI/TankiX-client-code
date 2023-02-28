using System.IO;
using SharpCompress.Common;

namespace SharpCompress.Archive {
    public static class AbstractWritableArchiveExtensions {
        public static void SaveTo<TEntry, TVolume>(this AbstractWritableArchive<TEntry, TVolume> writableArchive, Stream stream, CompressionType compressionType)
            where TEntry : IArchiveEntry where TVolume : IVolume {
            writableArchive.SaveTo(stream, new CompressionInfo {
                Type = compressionType
            });
        }
    }
}