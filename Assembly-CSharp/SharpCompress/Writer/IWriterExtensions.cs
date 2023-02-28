using System.IO;

namespace SharpCompress.Writer {
    public static class IWriterExtensions {
        public static void Write(this IWriter writer, string entryPath, Stream source) {
            writer.Write(entryPath, source, null);
        }
    }
}