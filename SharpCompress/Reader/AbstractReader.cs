using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Common;

namespace SharpCompress.Reader {
    public abstract class AbstractReader<TEntry, TVolume> : IDisposable, IStreamListener, IReader
        where TEntry : Entry where TVolume : Volume {
        bool completed;

        IEnumerator<TEntry> entriesForCurrentReadStream;

        bool wroteCurrentEntry;

        internal AbstractReader(Options options, ArchiveType archiveType) {
            ArchiveType = archiveType;
            Options = options;
        }

        internal Options Options { get; private set; }

        public abstract TVolume Volume { get; }

        public TEntry Entry => entriesForCurrentReadStream.Current;

        public void Dispose() {
            if (entriesForCurrentReadStream != null) {
                entriesForCurrentReadStream.Dispose();
            }

            TVolume volume = Volume;
            volume.Dispose();
        }

        IEntry IReader.Entry => Entry;

        public ArchiveType ArchiveType { get; }

        public event EventHandler<CompressedBytesReadEventArgs> CompressedBytesRead;

        public event EventHandler<FilePartExtractionBeginEventArgs> FilePartExtractionBegin;

        public bool MoveToNextEntry() {
            if (completed) {
                return false;
            }

            if (entriesForCurrentReadStream == null) {
                return LoadStreamForReading(RequestInitialStream());
            }

            if (!wroteCurrentEntry) {
                SkipEntry();
            }

            wroteCurrentEntry = false;

            if (NextEntryForCurrentStream()) {
                return true;
            }

            completed = true;
            return false;
        }

        public void WriteEntryTo(Stream writableStream) {
            if (wroteCurrentEntry) {
                throw new ArgumentException("WriteEntryTo or OpenEntryStream can only be called once.");
            }

            if (writableStream == null || !writableStream.CanWrite) {
                throw new ArgumentNullException("A writable Stream was required.  Use Cancel if that was intended.");
            }

            Write(writableStream);
            wroteCurrentEntry = true;
        }

        public EntryStream OpenEntryStream() {
            if (wroteCurrentEntry) {
                throw new ArgumentException("WriteEntryTo or OpenEntryStream can only be called once.");
            }

            EntryStream entryStream = GetEntryStream();
            wroteCurrentEntry = true;
            return entryStream;
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

        internal bool LoadStreamForReading(Stream stream) {
            if (entriesForCurrentReadStream != null) {
                entriesForCurrentReadStream.Dispose();
            }

            if (stream == null || !stream.CanRead) {
                TEntry entry = Entry;

                throw new MultipartStreamRequiredException("File is split into multiple archives: '" +
                                                           entry.FilePath +
                                                           "'. A new readable stream is required.  Use Cancel if it was intended.");
            }

            entriesForCurrentReadStream = GetEntries(stream).GetEnumerator();

            if (entriesForCurrentReadStream.MoveNext()) {
                return true;
            }

            return false;
        }

        internal virtual Stream RequestInitialStream() {
            TVolume volume = Volume;
            return volume.Stream;
        }

        internal virtual bool NextEntryForCurrentStream() => entriesForCurrentReadStream.MoveNext();

        internal abstract IEnumerable<TEntry> GetEntries(Stream stream);

        void SkipEntry() {
            TEntry entry = Entry;

            if (!entry.IsDirectory) {
                Skip();
            }
        }

        internal void Skip() {
            byte[] array = new byte[4096];

            using (Stream stream = OpenEntryStream()) {
                while (stream.Read(array, 0, array.Length) > 0) { }
            }
        }

        internal void Write(Stream writeStream) {
            using (Stream source = OpenEntryStream()) {
                source.TransferTo(writeStream);
            }
        }

        protected virtual EntryStream GetEntryStream() {
            TEntry entry = Entry;
            return new EntryStream(entry.Parts.First().GetStream());
        }
    }
}