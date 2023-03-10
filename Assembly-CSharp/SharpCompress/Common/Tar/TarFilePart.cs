using System.IO;
using SharpCompress.Common.Tar.Headers;
using SharpCompress.IO;

namespace SharpCompress.Common.Tar {
    internal class TarFilePart : FilePart {
        readonly Stream seekableStream;

        internal TarFilePart(TarHeader header, Stream seekableStream) {
            this.seekableStream = seekableStream;
            Header = header;
        }

        internal TarHeader Header { get; }

        internal override string FilePartName => Header.Name;

        internal override Stream GetStream() {
            if (seekableStream != null) {
                seekableStream.Position = Header.DataStartPosition.Value;
                return new ReadOnlySubStream(seekableStream, Header.Size);
            }

            return Header.PackedStream;
        }
    }
}