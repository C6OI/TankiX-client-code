using System;

namespace SharpCompress.Common {
    public class CompressedBytesReadEventArgs : EventArgs {
        public long CompressedBytesRead { get; internal set; }

        public long CurrentFilePartCompressedBytesRead { get; internal set; }
    }
}