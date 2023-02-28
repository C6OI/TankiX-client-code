using System;
using System.Collections.Generic;
using log4net;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Activator = Platform.Kernel.OSGi.ClientCore.API.Activator;

namespace Tanks.ClientLauncher {
    public class ActivatorsLauncher {
        readonly Queue<Activator> activators;
        readonly ILog logger;

        public ActivatorsLauncher(IEnumerable<Activator> activators) {
            this.activators = new Queue<Activator>(activators);
            logger = LoggerProvider.GetLogger<ActivatorsLauncher>();
        }

        public void LaunchAll(Action onComplete = null) {
            InjectionUtils.RegisterInjectionPoints(typeof(InjectAttribute), ServiceRegistry.Current);
            LaunchECSActivators();
            LaunchActivator(onComplete);
        }

        void LaunchECSActivators() {
            foreach (Activator activator in activators) {
                if (activator is ECSActivator) {
                    logger.InfoFormat("Activate ECS part {0}", activator.GetType());
                    ((ECSActivator)activator).RegisterSystemsAndTemplates();
                }
            }
        }

        public void LaunchActivator(Action onComplete = null) {
            if (activators.Count > 0) {
                Activator activator = activators.Dequeue();
                logger.InfoFormat("Activate {0}", activator.GetType());

                activator.Launch(delegate {
                    LaunchActivator(onComplete);
                });
            } else if (onComplete != null) {
                onComplete();
            }
        }
    }
}