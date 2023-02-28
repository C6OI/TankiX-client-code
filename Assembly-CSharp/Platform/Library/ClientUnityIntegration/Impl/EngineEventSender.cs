using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.Library.ClientUnityIntegration.Impl {
    public static class EngineEventSender {
        public static void SendEventIntoEngine(EngineServiceInternal engineServiceInternal, Event e) {
            ICollection<Entity> allEntities = engineServiceInternal.EntityRegistry.GetAllEntities();
            IEnumerator<Entity> enumerator = allEntities.GetEnumerator();

            while (enumerator.MoveNext()) {
                Flow.Current.SendEvent(e, enumerator.Current);
            }
        }
    }
}