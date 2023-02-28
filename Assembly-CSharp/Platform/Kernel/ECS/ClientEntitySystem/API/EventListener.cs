using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface EventListener {
        void OnEventSend(Event evt, ICollection<Entity> entities);
    }
}