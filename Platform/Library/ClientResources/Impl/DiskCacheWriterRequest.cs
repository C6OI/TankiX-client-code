namespace Platform.Library.ClientResources.Impl {
    public class DiskCacheWriterRequest {
        public string Path { get; set; }

        public byte[] Data { get; set; }

        public bool IsDone { get; set; }

        public string Error { get; set; }
    }
}