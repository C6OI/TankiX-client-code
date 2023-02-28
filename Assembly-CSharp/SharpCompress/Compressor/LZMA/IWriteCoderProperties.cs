using System.IO;

namespace SharpCompress.Compressor.LZMA {
    internal interface IWriteCoderProperties {
        void WriteCoderProperties(Stream outStream);
    }
}