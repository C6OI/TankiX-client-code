using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.Impl {
    public class ElementLockSystem : ECSSystem {
        [OnEventFire]
        public void LockElement(LockElementEvent e, Node node,
            [Combine] [JoinAll] SingleNode<LockedElementComponent> lockedElement) =>
            lockedElement.component.canvasGroup.blocksRaycasts = false;

        [OnEventFire]
        public void UnlockElement(UnlockElementEvent e, Node node,
            [Combine] [JoinAll] SingleNode<LockedElementComponent> lockedElement) =>
            lockedElement.component.canvasGroup.blocksRaycasts = true;
    }
}