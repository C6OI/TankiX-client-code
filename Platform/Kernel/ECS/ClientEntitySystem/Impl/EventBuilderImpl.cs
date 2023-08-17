using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EventBuilderImpl : EventBuilder {
        readonly List<Entity> entities = new();
        DelayedEventManager delayedEventManager;

        Event eventInstance;

        Flow flow;

        public EventBuilder Attach(Entity entity) {
            if (entity == null) {
                throw new NullEntityException();
            }

            entities.Add(entity);
            return this;
        }

        public EventBuilder Attach(Node node) => Attach(node.Entity);

        public EventBuilder Attach(ICollection<Node> nodes) {
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator(entities);

            while (enumerator.MoveNext()) {
                Attach(enumerator.Current);
            }

            return null;
        }

        public EventBuilder AttachAll(ICollection<Entity> entities) {
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator(entities);

            while (enumerator.MoveNext()) {
                this.entities.Add(enumerator.Current);
            }

            return this;
        }

        public EventBuilder AttachAll(params Entity[] entities) {
            for (int i = 0; i < entities.Length; i++) {
                this.entities.Add(entities[i]);
            }

            return this;
        }

        public EventBuilder AttachAll(params Node[] nodes) {
            for (int i = 0; i < nodes.Length; i++) {
                Attach(nodes[i]);
            }

            return this;
        }

        public void Schedule() => flow.SendEvent(eventInstance, entities);

        public ScheduledEvent ScheduleDelayed(float timeInSec) {
            ScheduleManager scheduleManager = delayedEventManager.ScheduleDelayedEvent(eventInstance, entities, timeInSec);
            return new ScheduledEventImpl(eventInstance, scheduleManager);
        }

        public ScheduledEvent SchedulePeriodic(float timeInSec) {
            ScheduleManager scheduleManager = delayedEventManager.SchedulePeriodicEvent(eventInstance, entities, timeInSec);
            return new ScheduledEventImpl(eventInstance, scheduleManager);
        }

        public EventBuilderImpl Init(DelayedEventManager delayedEventManager, Flow flow, Event eventInstance) {
            this.delayedEventManager = delayedEventManager;
            this.flow = flow;
            this.eventInstance = eventInstance;
            entities.Clear();
            return this;
        }
    }
}