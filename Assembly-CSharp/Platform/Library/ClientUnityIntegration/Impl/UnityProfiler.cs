using System;
using System.Linq;
using UnityEngine.Profiling;

namespace Platform.Library.ClientUnityIntegration.Impl {
    public static class UnityProfiler {
        public static void Listen() {
            string[] commandLineArgs = Environment.GetCommandLineArgs();

            if (commandLineArgs != null && commandLineArgs.Any(arg => "-profiler".Equals(arg))) {
                Profiler.enabled = true;
            }

            Kernel.OSGi.ClientCore.API.Profiler.OnBeginSample -= OnBeginSample;
            Kernel.OSGi.ClientCore.API.Profiler.OnBeginSample += OnBeginSample;
            Kernel.OSGi.ClientCore.API.Profiler.OnEndSample -= OnEndSample;
            Kernel.OSGi.ClientCore.API.Profiler.OnEndSample += OnEndSample;
        }

        public static void OnBeginSample(string name) { }

        public static void OnEndSample() { }
    }
}