using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    internal class FireOnlyEventSender : EventSender {
        readonly ICollection<Handler> fireHandlers;

        internal FireOnlyEventSender(ICollection<Handler> fireHandlers) => this.fireHandlers = fireHandlers;

        public void Send(Flow flow, Event e, ICollection<Entity> entities) {
            flow.TryInvoke(fireHandlers, e, entities);
        }
    }
}