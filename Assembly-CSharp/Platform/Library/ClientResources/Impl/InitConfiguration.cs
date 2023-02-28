namespace Platform.Library.ClientResources.Impl {
    public class InitConfiguration {
        public static InitConfiguration Config { get; set; }

        public string ConfigVersion { get; set; }

        public string BundleDbVersion { get; set; }

        public string Host { get; set; }

        public string AcceptorPort { get; set; }

        public string ResourcesUrl { get; set; }

        public string ConfigsUrl { get; set; }

        public string UpdateConfigUrl { get; set; }

        public string StateFileUrl { get; set; }
    }
}