using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class ECSNotRunningException : Exception {
        public ECSNotRunningException(string str)
            : base(str) { }

        public ECSNotRunningException() { }
    }
}