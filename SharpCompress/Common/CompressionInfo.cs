using SharpCompress.Compressor.Deflate;

namespace SharpCompress.Common {
    public class CompressionInfo {
        public CompressionInfo() => DeflateCompressionLevel = CompressionLevel.Default;

        public CompressionType Type { get; set; }

        public CompressionLevel DeflateCompressionLevel { get; set; }

        public static implicit operator CompressionInfo(CompressionType compressionType) {
            CompressionInfo compressionInfo = new();
            compressionInfo.Type = compressionType;
            return compressionInfo;
        }
    }
}