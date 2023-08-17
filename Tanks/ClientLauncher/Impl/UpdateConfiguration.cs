namespace Tanks.ClientLauncher.Impl {
    public class UpdateConfiguration {
        public string LastClientVersion { get; set; }

        public string DistributionUrl { get; set; }

        public string Executable { get; set; }

        public static UpdateConfiguration Config { get; set; }
    }
}