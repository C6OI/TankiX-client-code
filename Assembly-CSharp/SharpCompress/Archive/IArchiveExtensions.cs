using System.Linq;
using SharpCompress.Common;

namespace SharpCompress.Archive {
    public static class IArchiveExtensions {
        public static void WriteToDirectory(this IArchive archive, string destinationDirectory, ExtractOptions options = ExtractOptions.Overwrite) {
            foreach (IArchiveEntry item in archive.Entries.Where(x => !x.IsDirectory)) {
                item.WriteToDirectory(destinationDirectory, options);
            }
        }
    }
}