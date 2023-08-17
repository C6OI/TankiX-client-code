namespace log4net {
    public sealed class AssemblyInfo {
        public const string Version = "1.2.13";

        public const decimal TargetFrameworkVersion = 1.0m;

        public const string TargetFramework = "Mono";

        public const bool ClientProfile = false;

        public static string Info => string.Format("Apache log4net version {0} compiled for {1}{2} {3}",
            "1.2.13",
            "Mono",
            string.Empty,
            1.0m);
    }
}