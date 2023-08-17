using System.IO;
using SharpCompress.Common;

namespace SharpCompress.Archive {
    public static class IArchiveEntryExtensions {
        public static void WriteToDirectory(this IArchiveEntry entry, string destinationDirectory,
            ExtractOptions options = ExtractOptions.Overwrite) {
            string empty = string.Empty;
            string fileName = Path.GetFileName(entry.FilePath);

            if (options.HasFlag(ExtractOptions.ExtractFullPath)) {
                string directoryName = Path.GetDirectoryName(entry.FilePath);
                string text = Path.Combine(destinationDirectory, directoryName);

                if (!Directory.Exists(text)) {
                    Directory.CreateDirectory(text);
                }

                empty = Path.Combine(text, fileName);
            } else {
                empty = Path.Combine(destinationDirectory, fileName);
            }

            entry.WriteToFile(empty, options);
        }

        public static void WriteToFile(this IArchiveEntry entry, string destinationFileName,
            ExtractOptions options = ExtractOptions.Overwrite) {
            if (entry.IsDirectory) {
                return;
            }

            FileMode mode = FileMode.Create;

            if (!options.HasFlag(ExtractOptions.Overwrite)) {
                mode = FileMode.CreateNew;
            }

            using (FileStream stream = File.Open(destinationFileName, mode)) {
                entry.WriteTo(stream);
            }
        }
    }
}