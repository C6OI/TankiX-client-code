using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class EventCommandCollector : AbstractCommandCollector, EventListener {
        readonly ComponentAndEventRegistrator componentAndEventRegistrator;

        readonly SharedEntityRegistry entityRegistry;

        public EventCommandCollector(CommandCollector commandCollector, ComponentAndEventRegistrator componentAndEventRegistrator, SharedEntityRegistry entityRegistry)
            : base(commandCollector) {
            this.componentAndEventRegistrator = componentAndEventRegistrator;
            this.entityRegistry = entityRegistry;
        }

        public void OnEventSend(Event evt, ICollection<Entity> entities) {
            if (!evt.GetType().IsDefined(typeof(Shared), true)) {
                return;
            }

            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator(entities);
            object[] instanceArray = Cache.array.GetInstanceArray(entities.Count);
            int num = 0;

            while (enumerator.MoveNext()) {
                Entity current = enumerator.Current;

                if (entityRegistry.IsShared(current.Id)) {
                    instanceArray[num++] = current;
                }
            }

            if (num > 0) {
                Entity[] instanceArray2 = Cache.entityArray.GetInstanceArray(num);
                Array.Copy(instanceArray, instanceArray2, num);
                AddCommand(new SendEventCommand().Init(instanceArray2, evt));
            }
        }
    }
}