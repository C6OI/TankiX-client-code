using System.IO;

namespace SharpCompress.Compressor.LZMA {
    internal interface ICoder {
        void Code(Stream inStream, Stream outStream, long inSize, long outSize, ICodeProgress progress);
    }
}