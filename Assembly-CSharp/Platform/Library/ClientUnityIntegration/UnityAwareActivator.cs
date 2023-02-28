using System;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientUnityIntegration.API;
using Activator = Platform.Kernel.OSGi.ClientCore.API.Activator;

namespace Platform.Library.ClientUnityIntegration {
    public abstract class UnityAwareActivator<TCompletionStrategy> : ECSBehaviour, Activator where TCompletionStrategy : ActivatorCompletionStrategy, new() {
        Action onComplete;

        public void OnEnable() { }

        public void Launch(Action onComplete = null) {
            LoggerProvider.GetLogger(this).InfoFormat("Activate {0}", this);
            this.onComplete = onComplete;
            Activate();
            new TCompletionStrategy().TryAutoCompletion(Complete);
        }

        protected void Complete() {
            LoggerProvider.GetLogger(this).InfoFormat("Complete {0}", this);

            if (onComplete != null) {
                onComplete();
            }
        }

        protected virtual void Activate() { }
    }
}