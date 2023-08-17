using System;

namespace log4net.Core {
    [Flags]
    public enum FixFlags {
        [Obsolete("Replaced by composite Properties")]
        Mdc = 1,
        Ndc = 2,
        Message = 4,
        ThreadName = 8,
        LocationInfo = 0x10,
        UserName = 0x20,
        Domain = 0x40,
        Identity = 0x80,
        Exception = 0x100,
        Properties = 0x200,
        None = 0,
        All = 0xFFFFFFF,
        Partial = 0x34C
    }
}