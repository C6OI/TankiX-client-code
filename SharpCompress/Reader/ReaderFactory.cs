using System;
using System.IO;
using SharpCompress.Archive.Tar;
using SharpCompress.Common;
using SharpCompress.Compressor;
using SharpCompress.Compressor.BZip2;
using SharpCompress.IO;
using SharpCompress.Reader.Tar;

namespace SharpCompress.Reader {
    public static class ReaderFactory {
        public static IReader Open(Stream stream, Options options = Options.KeepStreamsOpen) {
            stream.CheckNotNull("stream");
            RewindableStream rewindableStream = new(stream);
            rewindableStream.StartRecording();
            rewindableStream.Rewind(false);

            if (BZip2Stream.IsBZip2(rewindableStream)) {
                rewindableStream.Rewind(false);
                BZip2Stream stream2 = new(rewindableStream, CompressionMode.Decompress);

                if (TarArchive.IsTarFile(stream2)) {
                    rewindableStream.Rewind(true);
                    return new TarReader(rewindableStream, CompressionType.BZip2, options);
                }
            }

            rewindableStream.Rewind(false);

            if (TarArchive.IsTarFile(rewindableStream)) {
                rewindableStream.Rewind(true);
                return TarReader.Open(rewindableStream, options);
            }

            throw new InvalidOperationException("Cannot determine compressed stream type.");
        }
    }
}