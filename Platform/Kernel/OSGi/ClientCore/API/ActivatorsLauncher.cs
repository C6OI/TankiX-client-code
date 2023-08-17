using System;
using System.Collections.Generic;

namespace Platform.Kernel.OSGi.ClientCore.API {
    public class ActivatorsLauncher {
        readonly Queue<Activator> activators;

        public ActivatorsLauncher(IEnumerable<Activator> activators) => this.activators = new Queue<Activator>(activators);

        public void LaunchAll(Action onComplete = null) => LaunchActivator(onComplete);

        void LaunchActivator(Action onComplete = null) {
            if (activators.Count > 0) {
                Activator activator = activators.Dequeue();

                activator.Launch(delegate {
                    LaunchActivator(onComplete);
                });
            } else if (onComplete != null) {
                onComplete();
            }
        }
    }
}