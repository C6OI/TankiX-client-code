using System;
using System.Diagnostics;

namespace Platform.Kernel.OSGi.ClientCore.API {
    public static class Profiler {
        public static event Action<string> OnBeginSample;

        public static event Action OnEndSample;

        [Conditional("ENABLE_PROFILER")]
        public static void BeginSample(string name) {
            if (OnBeginSample != null) {
                OnBeginSample(name);
            }
        }

        [Conditional("ENABLE_PROFILER")]
        public static void EndSample() {
            if (OnEndSample != null) {
                OnEndSample();
            }
        }
    }
}