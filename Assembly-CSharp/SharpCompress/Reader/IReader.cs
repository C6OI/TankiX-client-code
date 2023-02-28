using System;
using System.IO;
using SharpCompress.Common;

namespace SharpCompress.Reader {
    public interface IReader : IDisposable {
        ArchiveType ArchiveType { get; }

        IEntry Entry { get; }

        event EventHandler<CompressedBytesReadEventArgs> CompressedBytesRead;

        event EventHandler<FilePartExtractionBeginEventArgs> FilePartExtractionBegin;

        void WriteEntryTo(Stream writableStream);

        bool MoveToNextEntry();

        EntryStream OpenEntryStream();
    }
}