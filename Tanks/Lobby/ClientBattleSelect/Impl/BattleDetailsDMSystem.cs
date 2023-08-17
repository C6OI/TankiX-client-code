using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleDetailsDMSystem : ECSSystem {
        [OnEventFire]
        public void ShowDMInfoPanel(NodeAddedEvent e, BattleDMNode battleDm, [JoinByBattle] [Context] ScreenNode screen) =>
            screen.battleSelectScreen.DMInfoPanel.gameObject.SetActive(true);

        [OnEventFire]
        public void HideDMInfoPanel(NodeRemoveEvent e, BattleDMNode battleDm, ScreenNode screen) =>
            screen.battleSelectScreen.DMInfoPanel.gameObject.SetActive(false);

        public class BattleDMNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;
            public DMComponent dm;

            public SelectedListItemComponent selectedListItem;

            public UserLimitComponent userLimit;
        }

        public class ScreenNode : Node {
            public BattleGroupComponent battleGroup;
            public BattleSelectScreenComponent battleSelectScreen;
        }
    }
}