using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class ScreenLockSystem : ECSSystem {
        long currentLockedScreenId;

        [OnEventFire]
        public void FixLock(NodeAddedEvent e, SingleNode<ScreenComponent> screen, [JoinAll] SingleNode<ScreenLockComponent> screenLock) {
            if (currentLockedScreenId != screen.Entity.Id) {
                screen.component.Unlock();
                screenLock.component.Unlock();
            }
        }

        [OnEventFire]
        public void LockScreen(NodeAddedEvent e, LockedScreenNode screen, [JoinAll] SingleNode<ScreenLockComponent> screenLock) {
            currentLockedScreenId = screen.Entity.Id;
            screen.screen.Lock();
            screenLock.component.Lock();
        }

        [OnEventFire]
        public void UnlockScreen(NodeRemoveEvent e, LockedScreenNode screen, [JoinAll] SingleNode<ScreenLockComponent> screenLock) {
            if (currentLockedScreenId == screen.Entity.Id) {
                screen.screen.Unlock();
                screenLock.component.Unlock();
            }
        }

        public class LockedScreenNode : Node {
            public LockedScreenComponent lockedScreen;

            public ScreenComponent screen;
        }
    }
}