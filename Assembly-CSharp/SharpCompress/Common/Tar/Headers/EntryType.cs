namespace SharpCompress.Common.Tar.Headers {
    internal enum EntryType : byte {
        File = 0,
        OldFile = 48,
        HardLink = 49,
        SymLink = 50,
        CharDevice = 51,
        BlockDevice = 52,
        Directory = 53,
        Fifo = 54,
        LongLink = 75,
        LongName = 76,
        SparseFile = 83,
        VolumeHeader = 86
    }
}