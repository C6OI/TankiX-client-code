using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class InvalidInvokeGraphException : Exception {
        public InvalidInvokeGraphException(Handler handler)
            : base(string.Format("Supposed handler call, but skipped {0}", handler)) { }
    }
}