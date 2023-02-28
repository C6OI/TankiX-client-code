using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class ElementLockSystem : ECSSystem {
        [OnEventFire]
        public void LockElement(LockElementEvent e, Node node, [JoinAll] [Combine] SingleNode<LockedElementComponent> lockedElement) {
            lockedElement.component.canvasGroup.blocksRaycasts = false;
        }

        [OnEventFire]
        public void UnlockElement(UnlockElementEvent e, Node node, [JoinAll] [Combine] SingleNode<LockedElementComponent> lockedElement) {
            lockedElement.component.canvasGroup.blocksRaycasts = true;
        }
    }
}