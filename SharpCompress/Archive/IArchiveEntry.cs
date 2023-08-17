using System.IO;
using SharpCompress.Common;

namespace SharpCompress.Archive {
    public interface IArchiveEntry : IEntry {
        bool IsComplete { get; }

        Stream OpenEntryStream();

        void WriteTo(Stream stream);
    }
}