using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class FPSAdjustmentSystem : ECSSystem {
        [OnEventFire]
        public void MarkSelfBattleUserAsNeedToStartFPSStabilization(NodeAddedEvent e,
            SingleNode<SelfBattleUserComponent> selfBattleUser) {
            FPSUtil.SetMaxTargetFrameRate();
            selfBattleUser.Entity.AddComponent<NeedStartFPSStabilizationComponent>();
        }

        [OnEventFire]
        public void CancelFPSAdjustment(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> battleUser,
            [JoinAll] SingleNode<FPSTunerComponent> fpsTuner) {
            fpsTuner.component.FPSStabilizator.Stop();
            fpsTuner.component.FPSUpper.Stop();
            FPSUtil.SetMaxTargetFrameRate();
        }

        [OnEventFire]
        public void Stabilize(NodeAddedEvent e, SingleNode<TankActiveStateComponent> tank,
            [JoinByUser] BattleUserNode battleUser, [JoinAll] SingleNode<FPSTunerComponent> fpsTuner) {
            battleUser.Entity.RemoveComponent<NeedStartFPSStabilizationComponent>();

            fpsTuner.component.FPSStabilizator.Stabilize(delegate {
                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                    engine.ScheduleEvent<TryToUpFPSEvent>(fpsTuner);
                });
            });
        }

        [OnEventFire]
        public void TryToUpFPS(TryToUpFPSEvent e, SingleNode<FPSTunerComponent> fpsTuner,
            [JoinAll] SingleNode<SelfBattleUserComponent> battleUser) => fpsTuner.component.FPSUpper.TryToUp(
            fpsTuner.component.upIterationShiftFPS,
            delegate {
                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                    engine.ScheduleEvent<TryToUpFPSEvent>(fpsTuner);
                });
            });

        public class BattleUserNode : Node {
            public BattleUserComponent battleUser;

            public NeedStartFPSStabilizationComponent needStartFpsStabilization;

            public UserGroupComponent userGroup;
        }
    }
}