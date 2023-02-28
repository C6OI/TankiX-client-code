using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class PeriodicEventTask : ScheduleManager {
        readonly HashSet<Entity> contextEntities;
        readonly Event e;

        readonly EngineServiceInternal engineService;

        readonly float timeInSec;

        bool canceled;

        double timeToExecute;

        public PeriodicEventTask(Event e, EngineServiceInternal engineService, ICollection<Entity> contextEntities, float timeInSec) {
            this.e = e;
            this.engineService = engineService;
            this.contextEntities = new HashSet<Entity>(contextEntities);
            this.timeInSec = timeInSec;
            NewPeriod();
        }

        public bool Cancel() {
            if (canceled) {
                return false;
            }

            canceled = true;
            return true;
        }

        void NewPeriod() {
            timeToExecute = timeInSec;
        }

        public void Update(double time) {
            while (timeToExecute <= time) {
                timeToExecute += timeInSec;
                Flow flow = engineService.GetFlow();
                flow.SendEvent(e, contextEntities);
            }
        }

        public bool IsCanceled() => canceled;
    }
}