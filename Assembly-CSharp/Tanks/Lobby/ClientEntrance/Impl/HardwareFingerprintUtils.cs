using UnityEngine;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public static class HardwareFingerprintUtils {
        static string hardwareFingerprint;

        public static string HardwareFingerprint {
            get => hardwareFingerprint ?? SystemInfo.deviceUniqueIdentifier;
            set => hardwareFingerprint = value;
        }
    }
}