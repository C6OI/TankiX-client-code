namespace SharpCompress.Compressor.LZMA {
    interface ISetCoderProperties {
        void SetCoderProperties(CoderPropID[] propIDs, object[] properties);
    }
}