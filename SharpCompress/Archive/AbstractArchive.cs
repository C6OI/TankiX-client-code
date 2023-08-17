using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common;
using SharpCompress.Reader;

namespace SharpCompress.Archive {
    public abstract class AbstractArchive<TEntry, TVolume> : IDisposable, IArchive, IStreamListener
        where TEntry : IArchiveEntry where TVolume : IVolume {
        readonly LazyReadOnlyCollection<TEntry> lazyEntries;
        readonly LazyReadOnlyCollection<TVolume> lazyVolumes;

        bool disposed;

        internal AbstractArchive(ArchiveType type, IEnumerable<Stream> streams, Options options) {
            Type = type;
            lazyVolumes = new LazyReadOnlyCollection<TVolume>(LoadVolumes(streams.Select(CheckStreams), options));
            lazyEntries = new LazyReadOnlyCollection<TEntry>(LoadEntries(Volumes));
        }

        internal AbstractArchive(ArchiveType type) {
            Type = type;
            lazyVolumes = new LazyReadOnlyCollection<TVolume>(Enumerable.Empty<TVolume>());
            lazyEntries = new LazyReadOnlyCollection<TEntry>(Enumerable.Empty<TEntry>());
        }

        public virtual ICollection<TEntry> Entries => lazyEntries;

        public ICollection<TVolume> Volumes => lazyVolumes;

        IEnumerable<IArchiveEntry> IArchive.Entries => lazyEntries.Cast<IArchiveEntry>();

        IEnumerable<IVolume> IArchive.Volumes => lazyVolumes.Cast<IVolume>();

        public long TotalSize => Entries.Aggregate(0L, (total, cf) => total + cf.CompressedSize);

        public ArchiveType Type { get; }

        public virtual bool IsSolid => false;

        public bool IsComplete {
            get {
                EnsureEntriesLoaded();
                return Entries.All(x => x.IsComplete);
            }
        }

        public event EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> EntryExtractionBegin;

        public event EventHandler<ArchiveExtractionEventArgs<IArchiveEntry>> EntryExtractionEnd;

        public event EventHandler<CompressedBytesReadEventArgs> CompressedBytesRead;

        public event EventHandler<FilePartExtractionBeginEventArgs> FilePartExtractionBegin;

        public IReader ExtractAllEntries() {
            EnsureEntriesLoaded();
            return CreateReaderForSolidExtraction();
        }

        public void Dispose() {
            if (!disposed) {
                lazyVolumes.ForEach(delegate(TVolume v) {
                    v.Dispose();
                });

                lazyEntries.GetLoaded().Cast<Entry>().ForEach(delegate(Entry x) {
                    x.Close();
                });

                disposed = true;
            }
        }

        void IStreamListener.FireCompressedBytesRead(long currentPartCompressedBytes, long compressedReadBytes) {
            if (CompressedBytesRead != null) {
                CompressedBytesRead(this,
                    new CompressedBytesReadEventArgs {
                        CurrentFilePartCompressedBytesRead = currentPartCompressedBytes,
                        CompressedBytesRead = compressedReadBytes
                    });
            }
        }

        void IStreamListener.FireFilePartExtractionBegin(string name, long size, long compressedSize) {
            if (FilePartExtractionBegin != null) {
                FilePartExtractionBegin(this,
                    new FilePartExtractionBeginEventArgs {
                        CompressedSize = compressedSize,
                        Size = size,
                        Name = name
                    });
            }
        }

        internal void FireEntryExtractionBegin(IArchiveEntry entry) {
            if (EntryExtractionBegin != null) {
                EntryExtractionBegin(this, new ArchiveExtractionEventArgs<IArchiveEntry>(entry));
            }
        }

        internal void FireEntryExtractionEnd(IArchiveEntry entry) {
            if (EntryExtractionEnd != null) {
                EntryExtractionEnd(this, new ArchiveExtractionEventArgs<IArchiveEntry>(entry));
            }
        }

        static Stream CheckStreams(Stream stream) {
            if (!stream.CanSeek || !stream.CanRead) {
                throw new ArgumentException("Archive streams must be Readable and Seekable");
            }

            return stream;
        }

        protected abstract IEnumerable<TVolume> LoadVolumes(IEnumerable<Stream> streams, Options options);

        protected abstract IEnumerable<TEntry> LoadEntries(IEnumerable<TVolume> volumes);

        internal void EnsureEntriesLoaded() {
            lazyEntries.EnsureFullyLoaded();
            lazyVolumes.EnsureFullyLoaded();
        }

        protected abstract IReader CreateReaderForSolidExtraction();
    }
}