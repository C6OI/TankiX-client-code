using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientMatchMaking.API;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class PlayButtonClickSystem : ECSSystem {
        [OnEventFire]
        public void ShowGameModeSelect(ButtonClickEvent e, PlayButtonViewSystem.NormalStateNode playButton, [JoinAll] ButtonNode button,
            [JoinAll] Optional<SingleNode<RankedBattleGUIComponent>> rankedModeButton) {
            SelectDefaultMatchMakingModeEvent selectDefaultMatchMakingModeEvent = new();
            ScheduleEvent(selectDefaultMatchMakingModeEvent, playButton);
            playButton.playButton.SearchingDefaultGameMode = selectDefaultMatchMakingModeEvent.DefaultMode.IsPresent();

            if (selectDefaultMatchMakingModeEvent.DefaultMode.IsPresent()) {
                button.esm.Esm.ChangeState<PlayButtonStates.SearchingState>();
                string name = selectDefaultMatchMakingModeEvent.DefaultMode.Get().GetComponent<DescriptionItemComponent>().Name;
                MainScreenComponent.Instance.ShowMatchSearching(name);
            } else if (rankedModeButton.IsPresent()) {
                rankedModeButton.Get().component.Click();
            } else {
                MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.PlayScreen);
            }
        }

        [OnEventFire]
        public void GoToMatchSearching(EnteredToMatchMakingEvent e, MatchMakingModeNode mode, [JoinAll] SingleNode<SelfUserComponent> user, [JoinAll] ButtonNode button) {
            user.Entity.RemoveComponentIfPresent<UserEnteringToMatchMakingComponent>();
            button.esm.Esm.ChangeState<PlayButtonStates.SearchingState>();
            MainScreenComponent.Instance.ShowMatchSearching(mode.descriptionItem.Name);
        }

        [OnEventFire]
        public void ExitedFromMatchMaking(ExitedFromMatchMakingEvent e, SingleNode<MatchMakingComponent> matchMaking, [JoinAll] ButtonNode button) {
            button.esm.Esm.ChangeState<PlayButtonStates.NormalState>();

            if (MainScreenComponent.Instance.gameObject.activeSelf && MainScreenComponent.Instance.GetCurrentPanel() == MainScreenComponent.MainScreens.MatchSearching) {
                if (!e.SelfAction || button.playButton.SearchingDefaultGameMode) {
                    MainScreenComponent.Instance.ShowHome();
                } else {
                    MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.PlayScreen);
                }
            }
        }

        [OnEventFire]
        public void SendStartCustomBattle(ButtonClickEvent e, SingleNode<PlayButtonStartCustomBattleStateComponent> button, [JoinAll] LobbyNode lobby) {
            ScheduleEvent<StartBattleEvent>(lobby);
        }

        [OnEventFire]
        public void SendReturnToCustomBattle(ButtonClickEvent e, SingleNode<PlayButtonReturnToBattleStateComponent> button, [JoinAll] LobbyNode lobby) {
            ScheduleEvent<ReturnToCustomBattleEvent>(lobby);
        }

        [OnEventFire]
        public void SendExitLobby(ButtonClickEvent e, ExitLobbyButtonNode button, [JoinAll] CustomLobbyNode lobby) {
            ScheduleEvent<ClientExitLobbyEvent>(lobby);
        }

        [OnEventFire]
        public void GoToLobby(ButtonClickEvent e, SingleNode<GoToLobbyButtonComponent> button) {
            MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.MatchLobby);
        }

        [OnEventFire]
        public void ShowOrHideSearching(ButtonClickEvent e, PlayButtonViewSystem.SearchingStateNode button) {
            MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.MatchSearching, false);
        }

        [OnEventFire]
        public void ShowOrHideLobby(ButtonClickEvent e, PlayButtonViewSystem.NotEnoughtPlayersStateNode button) {
            MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.MatchLobby);
        }

        [OnEventFire]
        public void ShowOrHideLobby(ButtonClickEvent e, PlayButtonViewSystem.CustomBattleStateNode button) {
            MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.MatchLobby);
        }

        [OnEventFire]
        public void ShowOrHideLobby(ButtonClickEvent e, PlayButtonViewSystem.MatchBeginTimerStateNode button) {
            MainScreenComponent.Instance.ShowOrHideScreen(MainScreenComponent.MainScreens.MatchLobby);
        }

        [OnEventFire]
        public void ShowEnergyShareScreen(ButtonClickEvent e, PlayButtonViewSystem.ShareEnergyStateNode button) {
            MainScreenComponent.Instance.ShowShareEnergyScreen();
        }

        public class ButtonNode : Node {
            public ESMComponent esm;
            public PlayButtonComponent playButton;
        }

        public class LobbyNode : Node {
            public BattleLobbyComponent battleLobby;

            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class CustomLobbyNode : LobbyNode {
            public CustomBattleLobbyComponent customBattleLobby;
        }

        public class ExitLobbyButtonNode : Node {
            public ButtonMappingComponent buttonMapping;
            public ExitBattleLobbyButtonComponent exitBattleLobbyButton;
        }

        public class UserEnteringToMMNode : Node {
            public SelfUserComponent selfUser;

            public UserEnteringToMatchMakingComponent userEnteringToMatchMaking;
        }

        public class MatchMakingModeNode : Node {
            public DescriptionItemComponent descriptionItem;
            public MatchMakingModeComponent matchMakingMode;
        }
    }
}