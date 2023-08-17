using System;

namespace Platform.Kernel.OSGi.ClientCore.API {
    public class AutoCompleting : ActivatorCompletionStrategy {
        public void TryAutoCompletion(Action onComplete) => onComplete();
    }
}