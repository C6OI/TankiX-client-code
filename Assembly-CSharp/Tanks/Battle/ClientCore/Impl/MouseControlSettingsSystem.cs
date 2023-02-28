using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class MouseControlSettingsSystem : ECSSystem {
        const float MouseSensivityRatio = 1.5f;

        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void InitWeaponRotationControl(NodeAddedEvent e, SelfBattleUser selfBattleUser, SingleNode<GameMouseSettingsComponent> settings) {
            selfBattleUser.mouseControlStateHolder.MouseControlAllowed = settings.component.MouseControlAllowed;
            selfBattleUser.mouseControlStateHolder.MouseControlEnable = settings.component.MouseControlAllowed;
            selfBattleUser.mouseControlStateHolder.MouseVerticalInverted = settings.component.MouseVerticalInverted;
            selfBattleUser.mouseControlStateHolder.MouseSensivity = settings.component.MouseSensivity * 1.5f;
        }

        public class SelfBattleUser : Node {
            public MouseControlStateHolderComponent mouseControlStateHolder;

            public SelfBattleUserComponent selfBattleUser;
        }
    }
}