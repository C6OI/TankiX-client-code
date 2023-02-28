using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class LocalDurationSystem : ECSSystem {
        [OnEventFire]
        public void ScheduleEventForDelete(NodeAddedEvent e, SingleNode<LocalDurationComponent> node) {
            LocalDurationComponent component = node.component;
            component.StartedTime = Date.Now;
            component.LocalDurationExpireEvent = NewEvent<LocalDurationExpireEvent>().Attach(node).ScheduleDelayed(node.component.Duration).Manager();
        }

        [OnEventFire]
        public void Cancel(NodeRemoveEvent e, SingleNode<LocalDurationComponent> node) {
            if (!node.component.IsComplete) {
                node.component.LocalDurationExpireEvent.Cancel();
            }
        }

        [OnEventComplete]
        public void RemoveLocalDurationComponent(LocalDurationExpireEvent e, SingleNode<LocalDurationComponent> node) {
            node.component.IsComplete = true;
            node.Entity.RemoveComponent<LocalDurationComponent>();
        }
    }
}