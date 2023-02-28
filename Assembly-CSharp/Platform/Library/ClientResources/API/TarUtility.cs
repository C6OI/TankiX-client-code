using System;
using System.IO;
using SharpCompress.Common;
using SharpCompress.Compressor;
using SharpCompress.Compressor.Deflate;
using SharpCompress.Reader;
using SharpCompress.Writer;

namespace Platform.Library.ClientResources.API {
    public static class TarUtility {
        public static void Compress(string fromPath, Stream stream, CompressionType compresType, Func<string, bool> filter = null) {
            using (IWriter writer = WriterFactory.Open(stream, ArchiveType.Tar, compresType)) {
                foreach (string file in FileUtils.GetFiles(fromPath, filter)) {
                    using (FileStream source = new(file, FileMode.Open, FileAccess.Read)) {
                        string filename = file.Substring(fromPath.Length + 1);
                        writer.Write(filename, source, DateTime.Now);
                    }
                }
            }
        }

        public static void Extract(Stream stream, string toPath) {
            GZipStream stream2 = new(stream, CompressionMode.Decompress);

            using (IReader reader = ReaderFactory.Open(stream2)) {
                while (reader.MoveToNextEntry()) {
                    if (!reader.Entry.IsDirectory) {
                        string path = toPath + "/" + reader.Entry.FilePath;
                        string directoryName = Path.GetDirectoryName(path);

                        if (!Directory.Exists(directoryName)) {
                            Directory.CreateDirectory(directoryName);
                        }

                        using (FileStream writableStream = new(path, FileMode.Create, FileAccess.Write)) {
                            reader.WriteEntryTo(writableStream);
                        }
                    }
                }
            }
        }
    }
}