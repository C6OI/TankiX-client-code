using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class FireAndCompleteEventSender : EventSender {
        readonly ICollection<Handler> completeHandlers;
        readonly ICollection<Handler> fireHandlers;

        internal FireAndCompleteEventSender(ICollection<Handler> fireHandlers, ICollection<Handler> completeHandlers) {
            this.fireHandlers = fireHandlers;
            this.completeHandlers = completeHandlers;
        }

        public void Send(Flow flow, Event e, ICollection<Entity> entities) {
            flow.TryInvoke(fireHandlers, e, entities);
            flow.TryInvoke(completeHandlers, e, entities);
        }
    }
}