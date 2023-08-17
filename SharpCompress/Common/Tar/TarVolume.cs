using System.IO;

namespace SharpCompress.Common.Tar {
    public class TarVolume : GenericVolume {
        public TarVolume(Stream stream, Options options)
            : base(stream, options) { }
    }
}