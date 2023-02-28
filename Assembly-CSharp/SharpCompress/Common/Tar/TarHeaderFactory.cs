using System.Collections.Generic;
using System.IO;
using SharpCompress.Common.Tar.Headers;
using SharpCompress.IO;

namespace SharpCompress.Common.Tar {
    internal static class TarHeaderFactory {
        internal static IEnumerable<TarHeader> ReadHeader(StreamingMode mode, Stream stream) {
            while (true) {
                TarHeader header2 = null;

                try {
                    BinaryReader binaryReader = new(stream);
                    header2 = new TarHeader();

                    if (!header2.Read(binaryReader)) {
                        break;
                    }

                    switch (mode) {
                        case StreamingMode.Seekable:
                            header2.DataStartPosition = binaryReader.BaseStream.Position;
                            binaryReader.BaseStream.Position += PadTo512(header2.Size);
                            break;

                        case StreamingMode.Streaming:
                            header2.PackedStream = new TarReadOnlySubStream(stream, header2.Size);
                            break;

                        default:
                            throw new InvalidFormatException("Invalid StreamingMode");
                    }
                } catch {
                    header2 = null;
                }

                yield return header2;
            }
        }

        static long PadTo512(long size) {
            int num = (int)(size % 512);

            if (num == 0) {
                return size;
            }

            return 512 - num + size;
        }
    }
}