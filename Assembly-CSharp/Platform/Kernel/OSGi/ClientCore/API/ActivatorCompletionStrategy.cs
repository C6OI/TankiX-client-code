using System;

namespace Platform.Kernel.OSGi.ClientCore.API {
    public interface ActivatorCompletionStrategy {
        void TryAutoCompletion(Action onComplete);
    }
}