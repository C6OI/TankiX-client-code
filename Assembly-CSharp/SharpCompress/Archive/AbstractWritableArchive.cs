using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common;

namespace SharpCompress.Archive {
    public abstract class AbstractWritableArchive<TEntry, TVolume> : AbstractArchive<TEntry, TVolume> where TEntry : IArchiveEntry where TVolume : IVolume {
        readonly bool anyNotWritable;

        readonly List<TEntry> modifiedEntries = new();
        readonly List<TEntry> newEntries = new();

        readonly List<TEntry> removedEntries = new();

        bool hasModifications;

        internal AbstractWritableArchive(ArchiveType type)
            : base(type) { }

        internal AbstractWritableArchive(ArchiveType type, IEnumerable<Stream> streams, Options options)
            : base(type, streams, options) {
            if (streams.Any(x => !x.CanWrite)) {
                anyNotWritable = true;
            }
        }

        public override ICollection<TEntry> Entries {
            get {
                if (hasModifications) {
                    return modifiedEntries;
                }

                return base.Entries;
            }
        }

        IEnumerable<TEntry> OldEntries {
            get { return base.Entries.Where(x => !removedEntries.Contains(x)); }
        }

        void CheckWritable() {
            if (anyNotWritable) {
                throw new ArchiveException("All Archive streams must be Writable to use Archive writing functionality.");
            }
        }

        void RebuildModifiedCollection() {
            hasModifications = true;
            modifiedEntries.Clear();
            modifiedEntries.AddRange(OldEntries.Concat(newEntries));
        }

        public void RemoveEntry(TEntry entry) {
            CheckWritable();

            if (!removedEntries.Contains(entry)) {
                removedEntries.Add(entry);
                RebuildModifiedCollection();
            }
        }

        public void AddEntry(string filePath, Stream source, long size = 0L, DateTime? modified = null) {
            CheckWritable();
            newEntries.Add(CreateEntry(filePath, source, size, modified, false));
            RebuildModifiedCollection();
        }

        public void AddEntry(string filePath, Stream source, bool closeStream, long size = 0L, DateTime? modified = null) {
            CheckWritable();
            newEntries.Add(CreateEntry(filePath, source, size, modified, closeStream));
            RebuildModifiedCollection();
        }

        public void SaveTo(Stream stream, CompressionInfo compressionType) {
            SaveTo(stream, compressionType, OldEntries, newEntries);
        }

        protected abstract TEntry CreateEntry(string filePath, Stream source, long size, DateTime? modified, bool closeStream);

        protected abstract void SaveTo(Stream stream, CompressionInfo compressionType, IEnumerable<TEntry> oldEntries, IEnumerable<TEntry> newEntries);
    }
}