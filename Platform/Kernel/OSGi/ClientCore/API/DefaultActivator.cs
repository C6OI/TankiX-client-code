using System;

namespace Platform.Kernel.OSGi.ClientCore.API {
    public abstract class DefaultActivator<TCompletionStrategy> : Activator
        where TCompletionStrategy : ActivatorCompletionStrategy, new() {
        Action onComplete;

        public void Launch(Action onComplete = null) {
            this.onComplete = onComplete;
            Activate();
            new TCompletionStrategy().TryAutoCompletion(Complete);
        }

        protected void Complete() {
            if (onComplete != null) {
                onComplete();
            }
        }

        protected abstract void Activate();
    }
}