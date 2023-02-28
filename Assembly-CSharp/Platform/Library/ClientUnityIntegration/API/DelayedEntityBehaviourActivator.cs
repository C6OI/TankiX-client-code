using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Library.ClientUnityIntegration.API {
    public class DelayedEntityBehaviourActivator : UnityAwareActivator<AutoCompleting> {
        public readonly List<EntityBehaviour> DelayedEntityBehaviours = new(16);

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        protected override void Activate() {
            if (!EngineService.IsRunning) {
                throw new Exception("Engine Service is not running!");
            }

            foreach (EntityBehaviour delayedEntityBehaviour in DelayedEntityBehaviours) {
                if (delayedEntityBehaviour.gameObject.activeInHierarchy) {
                    delayedEntityBehaviour.CreateEntity();
                }
            }

            DelayedEntityBehaviours.Clear();
        }
    }
}