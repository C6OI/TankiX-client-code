using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class DelayedEventTask : ScheduleManager {
        readonly Event e;

        readonly EngineServiceInternal engine;

        readonly HashSet<Entity> entities;

        readonly double timeToExecute;

        bool canceled;

        bool invoked;

        public DelayedEventTask(Event e, ICollection<Entity> entities, EngineServiceInternal engine, double timeToExecute) {
            this.e = e;
            this.entities = new HashSet<Entity>(entities);
            this.engine = engine;
            this.timeToExecute = timeToExecute;
        }

        public bool Cancel() {
            canceled = true;
            return invoked;
        }

        public bool Update(double time) {
            if (timeToExecute <= time) {
                Flow flow = engine.NewFlow();
                RemoveDeletedEntities();

                flow.StartWith(delegate {
                    flow.SendEvent(e, entities);
                });

                engine.ExecuteFlow(flow);
                invoked = true;
            }

            return invoked;
        }

        public void RemoveDeletedEntities() => entities.RemoveWhere(entity => !((EntityImpl)entity).Alive);

        public bool IsCanceled() => canceled;
    }
}