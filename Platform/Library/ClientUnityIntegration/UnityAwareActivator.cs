using System;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;
using Activator = Platform.Kernel.OSGi.ClientCore.API.Activator;

namespace Platform.Library.ClientUnityIntegration {
    public abstract class UnityAwareActivator<TCompletionStrategy> : MonoBehaviour, Activator
        where TCompletionStrategy : ActivatorCompletionStrategy, new() {
        Action onComplete;

        public void OnEnable() { }

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

        protected virtual void Activate() { }
    }
}