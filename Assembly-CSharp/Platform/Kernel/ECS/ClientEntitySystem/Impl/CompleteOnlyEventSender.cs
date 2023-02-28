using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class CompleteOnlyEventSender : EventSender {
        readonly ICollection<Handler> completeHandlers;

        internal CompleteOnlyEventSender(ICollection<Handler> completeHandlers) => this.completeHandlers = completeHandlers;

        public void Send(Flow flow, Event e, ICollection<Entity> entities) {
            flow.TryInvoke(completeHandlers, e, entities);
        }
    }
}