using System;
using UnityEngine;

namespace Platform.Library.ClientResources.API {
    public static class BuildTargetName {
        public static string GetName() {
            if (Application.platform == RuntimePlatform.WebGLPlayer) {
                return "WebGLPlayer";
            }

            if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
                return "StandaloneWindows";
            }

            if (Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.LinuxEditor) {
                return "StandaloneLinux64";
            }

            if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor) {
                return "StandaloneOSXIntel64";
            }

            if (Application.platform == RuntimePlatform.Android) {
                return "Android";
            }

            throw new Exception("Could not parse current platform " + Application.platform);
        }
    }
}