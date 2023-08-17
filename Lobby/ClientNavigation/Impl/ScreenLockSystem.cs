using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.Impl {
    public class ScreenLockSystem : ECSSystem {
        [OnEventFire]
        public void LockScreen(NodeAddedEvent e, LockedScreenNode screen,
            [JoinAll] SingleNode<ScreenLockComponent> screenLock) {
            screen.screen.Lock();
            screenLock.component.Lock();
        }

        [OnEventFire]
        public void UnlockScreen(NodeRemoveEvent e, LockedScreenNode screen,
            [JoinAll] SingleNode<ScreenLockComponent> screenLock) {
            screen.screen.Unlock();
            screenLock.component.Unlock();
        }

        public class LockedScreenNode : Node {
            public LockedScreenComponent lockedScreen;

            public ScreenComponent screen;
        }
    }
}