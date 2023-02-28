using System;

namespace SharpCompress.Common {
    [Flags]
    public enum ExtractOptions {
        None = 0,
        Overwrite = 1,
        ExtractFullPath = 2
    }
}