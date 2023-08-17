using System.IO;

namespace SharpCompress.Compressor.LZMA {
    interface IWriteCoderProperties {
        void WriteCoderProperties(Stream outStream);
    }
}