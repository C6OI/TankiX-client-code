using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface EventSender {
        void Send(Flow flow, Event e, ICollection<Entity> entities);
    }
}