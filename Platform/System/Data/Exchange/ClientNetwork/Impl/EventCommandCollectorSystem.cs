using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class EventCommandCollectorSystem : AbstractCommandCollectorSystem {
        readonly ComponentAndEventRegistrator componentAndEventRegistrator;

        readonly SharedEntityRegistry entityRegistry;

        public EventCommandCollectorSystem(CommandCollector commandCollector,
            ComponentAndEventRegistrator componentAndEventRegistrator, SharedEntityRegistry entityRegistry)
            : base(commandCollector) {
            this.componentAndEventRegistrator = componentAndEventRegistrator;
            this.entityRegistry = entityRegistry;
        }

        [OnEventFire]
        public void EventSend(InternalSendEvent e) {
            List<Entity> list = null;

            foreach (Entity entity in e.Entities) {
                if (entityRegistry.IsShared(entity.Id)) {
                    if (list == null) {
                        list = Cache.listEntity.GetInstance();
                    }

                    list.Add(entity);
                }
            }

            if (list != null) {
                AddCommand(new SendEventCommand(list.ToArray(), e.RealRealEvent));
            }
        }
    }
}