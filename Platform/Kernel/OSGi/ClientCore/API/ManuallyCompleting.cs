using System;

namespace Platform.Kernel.OSGi.ClientCore.API {
    public class ManuallyCompleting : ActivatorCompletionStrategy {
        public void TryAutoCompletion(Action onComplete) { }
    }
}