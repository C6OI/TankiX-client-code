using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;

namespace Platform.Library.ClientUnityIntegration.Impl {
    public static class EngineEventSender {
        public static void SendEventIntoEngine(EngineServiceInternal engineServiceInternal, Event e) {
            ICollection<Entity> entities = engineServiceInternal.EntityRegistry.GetAllEntities();

            ClientUnityIntegrationUtils.ExecuteInFlow(delegate {
                IEnumerator<Entity> enumerator = entities.GetEnumerator();

                while (enumerator.MoveNext()) {
                    Flow.Current.SendEvent(e, enumerator.Current);
                }
            });
        }
    }
}