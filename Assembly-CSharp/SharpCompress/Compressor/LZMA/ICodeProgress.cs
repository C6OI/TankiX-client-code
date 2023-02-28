namespace SharpCompress.Compressor.LZMA {
    internal interface ICodeProgress {
        void SetProgress(long inSize, long outSize);
    }
}