namespace SharpCompress.Compressor.LZMA {
    internal interface ISetCoderProperties {
        void SetCoderProperties(CoderPropID[] propIDs, object[] properties);
    }
}