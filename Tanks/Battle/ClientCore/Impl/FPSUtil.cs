using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public static class FPSUtil {
        public static void SetTargetFrameRate(int targetFrameRate) {
            Application.targetFrameRate = targetFrameRate;

            if (targetFrameRate == 30) {
                QualitySettings.vSyncCount = 2;
            }

            if (targetFrameRate == 60) {
                QualitySettings.vSyncCount = 1;
            }
        }

        public static void SetMaxTargetFrameRate() {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = -1;
        }
    }
}