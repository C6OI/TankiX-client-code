using System;

namespace SharpCompress.Common {
    [Flags]
    public enum Options {
        None = 0,
        KeepStreamsOpen = 1,
        LookForHeader = 2
    }
}