using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class InputManagerSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void ClearInputManager(ApplicationFocusEvent e, SingleNode<SelfUserComponent> user) {
            if (!e.IsFocused) {
                InputManager.ClearActions();
            }
        }

        [OnEventFire]
        public void ClearInputManager(NodeAddedEvent e, SingleNode<SelfBattleUserComponent> user) {
            InputManager.ClearActions();
        }

        [OnEventFire]
        public void ClearInputManager(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> user) {
            InputManager.ClearActions();
        }
    }
}