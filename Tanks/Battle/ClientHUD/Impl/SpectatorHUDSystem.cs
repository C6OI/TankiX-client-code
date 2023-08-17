using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class SpectatorHUDSystem : ECSSystem {
        [OnEventFire]
        public void GoBack(ButtonClickEvent e, SingleNode<SpectatorBackButtonComponent> button) =>
            ScheduleEvent<GoBackFromBattleEvent>(button.Entity);

        [OnEventFire]
        public void ChangeCameraState(SpectatorGoBackRequestEvent e, Node anyNode, [JoinAll] FreeCameraNode camera) =>
            ScheduleEvent<GoBackFromBattleEvent>(anyNode.Entity);

        public class FreeCameraNode : Node {
            public BattleCameraComponent battleCamera;
            public CameraComponent camera;

            public FreeCameraComponent freeCamera;

            public SpectatorCameraComponent spectatorCamera;
        }
    }
}