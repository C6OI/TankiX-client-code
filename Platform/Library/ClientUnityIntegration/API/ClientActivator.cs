using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.Impl;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.API {
    public abstract class ClientActivator : MonoBehaviour {
        List<Activator> coreActivators;

        List<Activator> nonCoreActivators;

        [Inject] public static EngineServiceInternal EngineServiceInternal { get; set; }

        public void ActivateClient(List<Activator> coreActivators, List<Activator> nonCoreActivators) {
            this.coreActivators = coreActivators;
            this.nonCoreActivators = nonCoreActivators;
            InjectionUtils.RegisterInjectionPoints(typeof(InjectAttribute), ServiceRegistry.Current);
            LaunchCoreActivators();
        }

        void LaunchCoreActivators() {
            ActivatorsLauncher activatorsLauncher = new(coreActivators);
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
            ActivatorsLauncher activatorsLauncher = new(nonCoreActivators);
            activatorsLauncher.LaunchAll(OnAllActivatorsLaunched);
        }

        void OnAllActivatorsLaunched() {
            gameObject.AddComponent<EngineBehaviour>();

            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                Entity entity = engine.CreateEntity("loader");
                engine.ScheduleEvent<ClientStartEvent>(entity);
            });
        }

        protected IEnumerable<Activator> GetActivatorsAddedInUnityEditor() =>
            from a in gameObject.GetComponentsInChildren<Activator>()
            where ((MonoBehaviour)a).enabled
            select a;
    }
}