using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    class BattleDetailsTDMSystem : ECSSystem {
        [OnEventFire]
        public void ShowTDMInfoPanel(NodeAddedEvent e, BattleTDMNode battleTDM, ScreenNode screen) {
            RectTransform component = screen.battleSelectScreen.BattleInfoPanelsContainer.GetComponent<RectTransform>();
            ResetVerticalScroll(component);
            screen.battleSelectScreen.TDMInfoPanel.gameObject.SetActive(true);
        }

        [OnEventFire]
        public void HideTDMInfoPanel(NodeRemoveEvent e, BattleTDMNode battleTDM, ScreenNode screen) =>
            screen.battleSelectScreen.TDMInfoPanel.gameObject.SetActive(false);

        void ResetVerticalScroll(RectTransform panelsContainer) =>
            panelsContainer.anchoredPosition = new Vector2(panelsContainer.anchoredPosition.x, 0f);

        public class BattleTDMNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;

            public SelectedListItemComponent selectedListItem;
            public TeamBattleComponent teamBattle;

            public UserLimitComponent userLimit;
        }

        public class ScreenNode : Node {
            public BattleSelectScreenComponent battleSelectScreen;
        }
    }
}