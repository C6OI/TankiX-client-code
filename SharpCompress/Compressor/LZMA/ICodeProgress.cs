namespace SharpCompress.Compressor.LZMA {
    interface ICodeProgress {
        void SetProgress(long inSize, long outSize);
    }
}