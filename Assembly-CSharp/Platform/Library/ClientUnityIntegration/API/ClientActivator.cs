using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.Impl;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public abstract class ClientActivator : MonoBehaviour {
        protected ActivatorsLauncher activatorsLauncher;
        List<Activator> coreActivators;

        List<Activator> nonCoreActivators;

        [Inject] public static EngineServiceInternal EngineServiceInternal { get; set; }

        public bool AllActivatorsLaunched { get; private set; }

        public void ActivateClient(List<Activator> coreActivators, List<Activator> nonCoreActivators) {
            this.coreActivators = coreActivators;
            this.nonCoreActivators = nonCoreActivators;
            InjectionUtils.RegisterInjectionPoints(typeof(InjectAttribute), ServiceRegistry.Current);
            LaunchCoreActivators();
        }

        void LaunchCoreActivators() {
            activatorsLauncher = new ActivatorsLauncher(coreActivators);
            activatorsLauncher.LaunchAll(OnCoreActivatorsLaunched);
        }

        void OnCoreActivatorsLaunched() {
            (from a in nonCoreActivators
             select a as ECSActivator
             into a
             where a != null
             select a).ForEach(delegate(ECSActivator a) {
                a.RegisterSystemsAndTemplates();
            });

            EngineServiceInternal.RunECSKernel();
            gameObject.AddComponent<PreciseTimeBehaviour>();
            gameObject.AddComponent<EngineBehaviour>();
            activatorsLauncher = new ActivatorsLauncher(nonCoreActivators);
            activatorsLauncher.LaunchAll(OnAllActivatorsLaunched);
        }

        void OnAllActivatorsLaunched() {
            AllActivatorsLaunched = true;
            Engine engine = EngineServiceInternal.Engine;
            Entity entity = engine.CreateEntity("loader");
            engine.ScheduleEvent<ClientStartEvent>(entity);
        }

        protected IEnumerable<Activator> GetActivatorsAddedInUnityEditor() => from a in gameObject.GetComponentsInChildren<Activator>()
                                                                              where ((MonoBehaviour)a).enabled
                                                                              select a;
    }
}