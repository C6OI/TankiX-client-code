using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleLobbyScreenSystem : ECSSystem {
        [Inject] public static ConfigurationService ConfiguratorService { get; set; }

        [OnEventFire]
        public void ShowBattleLobby(NodeAddedEvent e, BattleLobbyNode lobby) {
            MainScreenComponent.Instance.ShowHome();
            MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.MatchLobby);
        }

        [OnEventFire]
        public void LeaveBattleLobby(NodeRemoveEvent e, BattleLobbyNode lobby, [JoinAll] LobbyUINode lobbyUI) {
            MainScreenComponent.Instance.ShowHome();
        }

        [OnEventFire]
        public void OnMatchBeginning(NodeAddedEvent e, StartingLobbyNode lobby) {
            MainScreenComponent.Instance.ShowHome();
            MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.MatchLobby);
        }

        [OnEventFire]
        public void ShowHomeOrLobby(GoBackFromBattleScreensEvent e, Node node, [JoinAll] Optional<BattleLobbyNode> lobby) {
            MainScreenComponent.Instance.ShowHome();

            if (lobby.IsPresent()) {
                MainScreenComponent.Instance.ShowScreen(MainScreenComponent.MainScreens.MatchLobby);
            }
        }

        [OnEventFire]
        public void ShowCustomLobbyElements(NodeAddedEvent e, LobbyUINode lobbyUI, CustomLobbyNode lobby) {
            lobbyUI.matchLobbyGUI.ShowCustomLobbyElements(true);
            lobbyUI.matchLobbyGUI.ShowEditParamsButton(lobby.Entity.HasComponent<SelfComponent>(), !lobby.Entity.HasComponent<BattleGroupComponent>());
            ClientBattleParams @params = lobby.clientBattleParams.Params;
            CreateBattleScreenComponent createBattleScreen = lobbyUI.matchLobbyGUI.createBattleScreen;
            lobbyUI.matchLobbyGUI.paramTimeLimit.text = @params.TimeLimit + " " + createBattleScreen.minutesText;
            lobbyUI.matchLobbyGUI.paramFriendlyFire.text = !@params.FriendlyFire ? createBattleScreen.offText : createBattleScreen.onText;
            lobbyUI.matchLobbyGUI.enabledModules.text = @params.DisabledModules ? createBattleScreen.offText : createBattleScreen.onText;
        }

        [OnEventFire]
        public void EnableEditButtonOnBattleFinish(NodeRemoveEvent e, SingleNode<BattleGroupComponent> bg, [JoinSelf] CustomLobbyNode lobby, [JoinAll] LobbyUINode lobbyUI) {
            lobbyUI.matchLobbyGUI.ShowEditParamsButton(lobby.Entity.HasComponent<SelfComponent>(), true);
        }

        [OnEventFire]
        public void HideElements(NodeRemoveEvent e, LobbyUINode lobbyUI) {
            lobbyUI.matchLobbyGUI.ShowCustomLobbyElements(false);
            lobbyUI.matchLobbyGUI.ShowEditParamsButton(false, true);
            lobbyUI.matchLobbyGUI.ShowChat(false);
        }

        [OnEventFire]
        public void OnLobbyError(BattleLobbyEnterToBattleErrorEvent e, SingleNode<SelfUserComponent> user, [JoinAll] DialogsNode dialogs) {
            EnterToBattleErrorDialog enterToBattleErrorDialog = dialogs.dialogs60.Get<EnterToBattleErrorDialog>();
            enterToBattleErrorDialog.Show();
        }

        [OnEventFire]
        public void OnDialogConfirm(DialogConfirmEvent e, SingleNode<EnterToBattleErrorDialog> inviteToLobbyDialog) {
            MainScreenComponent.Instance.ClearHistory();
            MainScreenComponent.Instance.ShowHome();
        }

        public class BattleLobbyNode : Node {
            public BattleLobbyComponent battleLobby;

            public BattleLobbyGroupComponent battleLobbyGroup;
        }

        public class CustomLobbyNode : BattleLobbyNode {
            public ClientBattleParamsComponent clientBattleParams;
            public CustomBattleLobbyComponent customBattleLobby;

            public UserGroupComponent userGroup;
        }

        public class LobbyUINode : Node {
            public MatchLobbyGUIComponent matchLobbyGUI;

            public ScreenGroupComponent screenGroup;
        }

        public class StartingLobbyNode : Node {
            public LobbyStartingStateComponent lobbyStartingState;
        }

        public class DialogsNode : Node {
            public Dialogs60Component dialogs60;
        }
    }
}