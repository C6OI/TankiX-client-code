using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Library.ClientUnityIntegration.Impl {
    public static class UnityProfiler {
        public static void Listen() {
            Profiler.OnBeginSample += OnBeginSample;
            Profiler.OnEndSample += OnEndSample;
        }

        public static void OnBeginSample(string name) { }

        public static void OnEndSample() { }
    }
}