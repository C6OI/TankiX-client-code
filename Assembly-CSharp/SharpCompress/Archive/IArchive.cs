using System;
using System.Collections.Generic;
using SharpCompress.Common;
using SharpCompress.Reader;

namespace SharpCompress.Archive {
    public interface IArchive : IDisposable {
        IEnumerable<IArchiveEntry> Entries { get; }

        long TotalSize { get; }

        IEnumerable<IVolume> Volumes { get; }

        ArchiveType Type { get; }

        bool IsSolid { get; }

        bool IsComplete { get; }

        event EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> EntryExtractionBegin;

        event EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> EntryExtractionEnd;

        event EventHandler<CompressedBytesReadEventArgs> CompressedBytesRead;

        event EventHandler<FilePartExtractionBeginEventArgs> FilePartExtractionBegin;

        IReader ExtractAllEntries();
    }
}