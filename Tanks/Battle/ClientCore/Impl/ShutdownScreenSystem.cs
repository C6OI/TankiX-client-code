using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class ShutdownScreenSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent e, SingleNode<ServerShutdownComponent> shutdown) {
            float num = shutdown.component.StopDateForClient.UnityTime - Date.Now.UnityTime;

            if (num > 1f) {
                NewEvent(new PeriodicShutdownCheckEvent()).Attach(shutdown).ScheduleDelayed(1f);
            } else {
                ScheduleEvent<TechnicalWorkEvent>(shutdown);
            }
        }

        [OnEventFire]
        public void CheckShutdown(PeriodicShutdownCheckEvent e, SingleNode<ServerShutdownComponent> shutdown) {
            float num = shutdown.component.StopDateForClient.UnityTime - Date.Now.UnityTime;

            if (num < 1f) {
                ScheduleEvent<TechnicalWorkEvent>(shutdown);
            } else {
                NewEvent(new PeriodicShutdownCheckEvent()).Attach(shutdown).ScheduleDelayed(1f);
            }
        }
    }
}