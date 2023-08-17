using UnityEngine;

namespace Assets.platform.library.ClientResources.Scripts.API {
    public static class DiskCaching {
        static DiskCaching() => Enabled = Caching.ready;

        public static bool Enabled { get; set; }

        public static long MaximumAvailableDiskSpace {
            set => Caching.maximumAvailableDiskSpace = value;
        }

        public static int ExpirationDelay {
            set => Caching.expirationDelay = value;
        }

        public static bool CompressionEnambled {
            set => Caching.compressionEnabled = value;
        }
    }
}