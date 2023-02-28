using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleInfoEntranceSystem : ECSSystem {
        [OnEventFire]
        public void DisableBackButtonWhenLoadFail(EnterBattleRequestFailEvent e, SingleNode<SelfUserComponent> user) {
            ScheduleEvent<EnterBattleFailedEvent>(user);
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, ScreenNode screen) {
            HideAllButtons(screen.battleSelectScreen);
            LockAllButtons(screen.battleSelectScreen);
        }

        [OnEventFire]
        public void EnterBattle(ButtonClickEvent e, BattleEnterButtonNode button, [JoinAll] SelectedBattleNode battle, [JoinAll] ScreenNode screen) {
            ScheduleEvent(new EnterBattleRequestEvent(button.enterBattleButton.TeamColor) {
                Source = "BATTLES_LIST"
            }, battle);

            ScheduleEvent<EnterBattleAttemptEvent>(battle);
            LockAllButtons(screen.battleSelectScreen);
        }

        [OnEventFire]
        public void EnterBattleAsSpectator(ButtonClickEvent e, BattleEnterButtonAsSpectatorNode button, [JoinAll] SelectedBattleNode battle, [JoinAll] ScreenNode screen) {
            ScheduleEvent<EnterBattleAsSpectatorRequestEvent>(battle);
            ScheduleEvent<EnterBattleAttemptEvent>(battle);
            LockAllButtons(screen.battleSelectScreen);
        }

        [OnEventFire]
        public void ResetButtons(NodeRemoveEvent e, SelectedBattleNode battle, [JoinAll] ScreenNode screen) {
            HideAllButtons(screen.battleSelectScreen);
            LockAllButtons(screen.battleSelectScreen);
        }

        [OnEventFire]
        public void EnableSpectatorButton(NodeAddedEvent e, SelectedNotArchivedBattleNode battle, [JoinAll] ScreenNode screen) {
            screen.battleSelectScreen.EnterBattleAsSpectatorButton.SetActive(true);
            screen.battleSelectScreen.EnterBattleAsSpectatorButton.SetInteractable(true);
        }

        [OnEventFire]
        public void ShowDMButton(NodeAddedEvent e, SelectedNotArchivedDMNode battle, [JoinAll] ScreenNode screen) {
            GameObject enterBattleDMButton = screen.battleSelectScreen.EnterBattleDMButton;
            enterBattleDMButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(enterBattleDMButton);

            LinkSpectatorButtonForNavigation(screen.battleSelectScreen.EnterBattleAsSpectatorButton.GetComponent<Selectable>(), enterBattleDMButton.GetComponent<Selectable>(),
                enterBattleDMButton.GetComponent<Selectable>());
        }

        [OnEventFire]
        public void ShowTeamButtons(NodeAddedEvent e, SelectedNotArchivedTeamNode battle, [JoinAll] ScreenNode screen) {
            screen.battleSelectScreen.EnterBattleRedButton.SetActive(true);
            screen.battleSelectScreen.EnterBattleBlueButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(screen.battleSelectScreen.EnterBattleBlueButton);

            LinkSpectatorButtonForNavigation(screen.battleSelectScreen.EnterBattleAsSpectatorButton.GetComponent<Selectable>(),
                screen.battleSelectScreen.EnterBattleRedButton.GetComponent<Selectable>(), screen.battleSelectScreen.EnterBattleBlueButton.GetComponent<Selectable>());
        }

        [OnEventFire]
        public void LockDMButton(NodeAddedEvent e, SelectedArchivedBattleNode button, [JoinAll] ScreenNode screen) {
            HideAllButtons(screen.battleSelectScreen);
        }

        [OnEventFire]
        public void UnlockDMButton(NodeAddedEvent e, SelectedNotFullDMNode battle, [JoinAll] ScreenNode screen) {
            screen.battleSelectScreen.EnterBattleDMButton.SetInteractable(CanEnter(battle));
        }

        [OnEventFire]
        public void LockDMButton(NodeRemoveEvent e, SelectedNotFullDMNode battle, [JoinAll] ScreenNode screen) {
            screen.battleSelectScreen.EnterBattleDMButton.SetInteractable(false);
        }

        [OnEventFire]
        public void InitTeamButtonLock(NodeAddedEvent e, [Combine] TeamNode team, [JoinByBattle] [Context] BattleNode battle, [JoinByBattle] ScreenNode screen) {
            GetTeamButton(team, screen).SetInteractable(!team.Entity.HasComponent<FullTeamComponent>() && CanEnter(battle));
        }

        [OnEventFire]
        public void LockTeamButton(NodeAddedEvent e, FullTeamNode team, [JoinByBattle] ScreenNode screen) {
            GetTeamButton(team, screen).SetInteractable(false);
        }

        [OnEventFire]
        public void UnlockTeamButton(NodeRemoveEvent e, [Combine] FullTeamNode team, [JoinByBattle] [Context] BattleNode battle, [JoinByBattle] ScreenNode screen) {
            GetTeamButton(team, screen).SetInteractable(CanEnter(battle));
        }

        static GameObject GetTeamButton(TeamNode team, ScreenNode screen) {
            if (team.teamColor.TeamColor == TeamColor.RED) {
                return screen.battleSelectScreen.EnterBattleRedButton;
            }

            if (team.teamColor.TeamColor == TeamColor.BLUE) {
                return screen.battleSelectScreen.EnterBattleBlueButton;
            }

            throw new Exception("Team button not found: " + team.teamColor.TeamColor);
        }

        void LinkSpectatorButtonForNavigation(Selectable spectatorButton, Selectable up, Selectable down) {
            Navigation navigation = spectatorButton.navigation;
            navigation.selectOnDown = down;
            navigation.selectOnUp = up;
            spectatorButton.navigation = navigation;
        }

        void LockAllButtons(BattleSelectScreenComponent screenComponent) {
            screenComponent.EnterBattleDMButton.SetInteractable(false);
            screenComponent.EnterBattleRedButton.SetInteractable(false);
            screenComponent.EnterBattleBlueButton.SetInteractable(false);
            screenComponent.EnterBattleAsSpectatorButton.SetInteractable(false);
        }

        void HideAllButtons(BattleSelectScreenComponent screenComponent) {
            LockAllButtons(screenComponent);
            screenComponent.EnterBattleDMButton.SetActive(false);
            screenComponent.EnterBattleRedButton.SetActive(false);
            screenComponent.EnterBattleBlueButton.SetActive(false);
            screenComponent.EnterBattleAsSpectatorButton.SetActive(false);
        }

        bool CanEnter(BattleNode battle) => battle.personalBattleInfo.Info.CanEnter;

        public class BattleNode : Node {
            public BattleComponent battle;

            public BattleGroupComponent battleGroup;

            public PersonalBattleInfoComponent personalBattleInfo;
        }

        public class SelectedBattleNode : BattleNode {
            public SelectedListItemComponent selectedListItem;
        }

        public class SelectedNotArchivedBattleNode : SelectedBattleNode {
            public NotArchivedBattleComponent notArchivedBattle;
        }

        public class SelectedNotArchivedDMNode : SelectedNotArchivedBattleNode {
            public DMComponent dm;
        }

        public class SelectedNotArchivedTeamNode : SelectedNotArchivedBattleNode {
            public TeamBattleComponent teamBattle;
        }

        public class SelectedNotFullDMNode : SelectedNotArchivedDMNode {
            public NotFullBattleComponent notFullBattle;
        }

        public class SelectedArchivedBattleNode : BattleNode {
            public ArchivedBattleComponent archivedBattle;

            public SelectedListItemComponent selectedListItem;
        }

        public class BattleEnterButtonNode : Node {
            public ButtonMappingComponent buttonMapping;
            public EnterBattleButtonComponent enterBattleButton;
        }

        public class BattleEnterButtonAsSpectatorNode : Node {
            public ButtonMappingComponent buttonMapping;
            public EnterBattleAsSpectatorButtonComponent enterBattleAsSpectatorButton;
        }

        public class ScreenNode : Node {
            public BattleSelectScreenComponent battleSelectScreen;
        }

        public class TeamNode : Node {
            public BattleGroupComponent battleGroup;
            public TeamComponent team;

            public TeamColorComponent teamColor;

            public TeamGroupComponent teamGroup;
        }

        public class FullTeamNode : TeamNode {
            public FullTeamComponent fullTeam;
        }
    }
}