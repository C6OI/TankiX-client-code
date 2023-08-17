using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleSelectLoadSystem : ECSSystem {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        [OnEventComplete]
        public void AttachScreenToTargeBattle(ShowScreenEvent e, SingleNode<BattleGroupComponent> node,
            [JoinAll] UngroupedBattleSelectLoadScreenNode screen) =>
            screen.Entity.AddComponent(new BattleGroupComponent(node.component.Key));

        [OnEventFire]
        public void ShowBattleSelect(NodeAddedEvent e, BattleSelectLoadScreenNode screen,
            SingleNode<UserReadyForLobbyComponent> user) =>
            ScheduleEvent(new ShowBattleEvent(screen.battleGroup.Key), EngineService.EntityStub);

        public class BattleSelectLoadScreenNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleSelectLoadScreenComponent battleSelectLoadScreen;
        }

        [Not(typeof(BattleGroupComponent))]
        public class UngroupedBattleSelectLoadScreenNode : Node {
            public BattleSelectLoadScreenComponent battleSelectLoadScreen;
        }
    }
}