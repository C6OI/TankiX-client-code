using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class JoinToScreenBattleSystem : ECSSystem {
        [OnEventFire]
        public void Join(NodeAddedEvent e, [Combine] JoinToScreenBattleNode joinToScreenBattle,
            [JoinByScreen] [Context] ScreenNode screen, [JoinByBattle] BattleNode battle) =>
            battle.battleGroup.Attach(joinToScreenBattle.Entity);

        public class ScreenNode : Node {
            public BattleGroupComponent battleGroup;

            public ScreenComponent screen;

            public ScreenGroupComponent screenGroup;
        }

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;
        }

        [Not(typeof(BattleGroupComponent))]
        public class JoinToScreenBattleNode : Node {
            public JoinToScreenBattleComponent joinToScreenBattle;

            public ScreenGroupComponent screenGroup;
        }
    }
}