using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientMatchMaking.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class PlayButtonStateSystem : ECSSystem {
        [Inject] public static ConfigurationService ConfiguratorService { get; set; }

        [OnEventFire]
        public void InitPlayButton(NodeAddedEvent e, SingleNode<MainScreenComponent> screen, Optional<ButtonNode> pb) {
            if (!pb.IsPresent()) {
                GameObject playButton = screen.component.playButton;
                Entity entity = CreateEntity("PlayButtonEntity");
                playButton.GetComponent<EntityBehaviour>().BuildEntity(entity);
                ESMComponent eSMComponent = new();
                entity.AddComponent(eSMComponent);
                EntityStateMachine esm = eSMComponent.Esm;
                esm.AddState<PlayButtonStates.NormalState>();
                esm.AddState<PlayButtonStates.SearchingState>();
                esm.AddState<PlayButtonStates.EnteringLobbyState>();
                esm.AddState<PlayButtonStates.MatchBeginTimerState>();
                esm.AddState<PlayButtonStates.NotEnoughtPlayersState>();
                esm.AddState<PlayButtonStates.MatchBeginningState>();
                esm.AddState<PlayButtonStates.CustomBattleState>();
                esm.AddState<PlayButtonStates.StartCustomBattleState>();
                esm.AddState<PlayButtonStates.ReturnToBattleState>();
                esm.AddState<PlayButtonStates.EnergyShareScreenState>();
                esm.ChangeState<PlayButtonStates.NormalState>();
            }
        }

        bool IsCancelVisible(bool searching, Optional<SingleNode<ChosenMatchMackingModeComponent>> chosenMode) => searching && !IsNewbieMode(chosenMode);

        bool IsNewbieMode(Optional<SingleNode<ChosenMatchMackingModeComponent>> chosenMode) =>
            chosenMode.IsPresent() && chosenMode.Get().component.ModeEntity.HasComponent<MatchMakingDefaultModeComponent>();

        [OnEventFire]
        public void UpdatePlayButton(UpdatePlayButtonEvent e, ButtonNode button, [JoinAll] Optional<LobbyNode> lobby, [JoinAll] Optional<LobbyUINode> lobbyUI,
            [JoinAll] Optional<SingleNode<BattleScreenComponent>> battleScreen, [JoinAll] Optional<SingleNode<GameModeSelectScreenComponent>> gameModeSelectScreen,
            [JoinAll] Optional<SingleNode<ChosenMatchMackingModeComponent>> chosenMode, [JoinAll] Optional<SelfUserInSquadNode> selfUserInSquad,
            [JoinAll] Optional<ShareScreenStateNode> shareScreenStateNode, [JoinAll] SingleNode<SelfUserComponent> user) {
            bool flag = lobby.IsPresent();
            bool flag2 = lobbyUI.IsPresent();
            bool flag3 = flag && lobby.Get().Entity.HasComponent<BattleGroupComponent>();
            bool flag4 = flag && lobby.Get().Entity.HasComponent<CustomBattleLobbyComponent>();
            bool flag5 = flag && lobby.Get().Entity.HasComponent<SelfComponent>();
            bool flag6 = flag && lobby.Get().Entity.HasComponent<ClientMatchMakingLobbyStartTimeComponent>();
            bool flag7 = button.Entity.HasComponent<PlayerButtonSearchingStateComponent>() && !battleScreen.IsPresent();
            bool flag8 = flag && lobby.Get().Entity.HasComponent<ClientMatchMakingLobbyStartingComponent>();
            bool flag9 = flag && lobby.Get().Entity.HasComponent<LobbyStartingStateComponent>();
            bool flag10 = flag8 || flag9;
            user.Entity.HasComponent<MatchMakingUserComponent>();
            bool flag11 = shareScreenStateNode.IsPresent();

            Log.InfoFormat("UpdatePlayButton lobbyExists={0} onLobbyScreen={1} battleStarted={2} customLobby={3} ownLobby={4} hasStartTime={5} starting={6}", flag, flag2, flag3, flag4,
                flag5, flag6, flag10);

            button.playButton.ShowExitLobbyButton(flag && flag2);
            button.playButton.ShowGoToLobbyButton(flag && !flag2);
            button.playButton.InitializeMatchSearchingWaitTime(IsNewbieMode(chosenMode));
            bool show = IsCancelVisible(flag7, chosenMode) && (!selfUserInSquad.IsPresent() || selfUserInSquad.Get().Entity.HasComponent<SquadLeaderComponent>());
            button.playButton.ShowCancelButton(show);
            button.playButton.GetComponent<TooltipShowBehaviour>().enabled = false;

            if (flag11) {
                button.esm.Esm.ChangeState<PlayButtonStates.EnergyShareScreenState>();
                return;
            }

            if (!flag) {
                if (!flag7) {
                    button.esm.Esm.ChangeState<PlayButtonStates.NormalState>();

                    if (!gameModeSelectScreen.IsPresent()) {
                        button.playButton.GetComponent<TooltipShowBehaviour>().enabled = true;
                    }
                }

                return;
            }

            if (flag4) {
                button.esm.Esm.ChangeState<PlayButtonStates.CustomBattleState>();
            } else {
                button.esm.Esm.ChangeState<PlayButtonStates.NotEnoughtPlayersState>();
            }

            if (flag2) {
                if (flag3) {
                    button.esm.Esm.ChangeState<PlayButtonStates.ReturnToBattleState>();
                } else if (flag5) {
                    button.esm.Esm.ChangeState<PlayButtonStates.StartCustomBattleState>();
                }
            }

            if (flag6) {
                button.esm.Esm.ChangeState<PlayButtonStates.MatchBeginTimerState>();
                button.playButton.RunTheTimer(lobby.Get().Entity.GetComponent<ClientMatchMakingLobbyStartTimeComponent>().StartTime);
            }

            if (flag10) {
                button.esm.Esm.ChangeState<PlayButtonStates.MatchBeginningState>();
                Date startTime = !lobby.Get().Entity.HasComponent<LobbyStartingStateComponent>() ? Date.Now : lobby.Get().Entity.GetComponent<LobbyStartingStateComponent>().StartDate;
                bool matchBeginning = Date.Now.UnityTime < startTime.UnityTime;
                button.playButton.RunTheTimer(startTime, matchBeginning);
            }
        }

        [OnEventFire]
        public void OnExitFromMatchMaking(ExitFromMatchMakingEvent e, Node any, [JoinAll] ButtonNode button) {
            button.esm.Esm.ChangeState<PlayButtonStates.NormalState>();
        }

        [OnEventFire]
        public void UpdateOnAddBattle(NodeAddedEvent e, SingleNode<BattleScreenComponent> battleScreen, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnGameSelectScreen(NodeAddedEvent e, SingleNode<GameModeSelectScreenComponent> gameModeSelectScreen, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnGameSelectScreenRemove(NodeRemoveEvent e, SingleNode<GameModeSelectScreenComponent> gameModeSelectScreen, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnRemoveBattle(NodeRemoveEvent e, SingleNode<BattleScreenComponent> battleScreen, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnAddSelf(NodeAddedEvent e, SelfLobbyNode lobby, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnAddUI(NodeAddedEvent e, LobbyUINode lobbyUI, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnRemoveUI(NodeRemoveEvent e, LobbyUINode lobbyUI, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnAddBattle(NodeAddedEvent e, LobbyWithBattleNode lobby, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnRemoveBattle(NodeRemoveEvent e, LobbyWithBattleNode lobby, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnAddSearch(NodeAddedEvent e, ButtonSearchingStateNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnRemoveSearch(NodeRemoveEvent e, ButtonSearchingStateNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnTimerState(NodeAddedEvent e, LobbyWithStartTimeNode lobby, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnTimerState(NodeRemoveEvent e, LobbyWithStartTimeNode lobby, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnStartingState(NodeAddedEvent e, StartingMMLobbyNode lobby, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void UpdateOnStartingState(NodeAddedEvent e, StartingCustomLobbyNode lobby, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        void DelayUpdate(Node button) {
            NewEvent<UpdatePlayButtonEvent>().Attach(button).ScheduleDelayed(0f);
        }

        [OnEventFire]
        public void UserAdded(NodeAddedEvent e, UserNode user, [JoinByBattleLobby] CustomLobbyNode lobby, [JoinByBattleLobby] ICollection<UserNode> allLobbyUsers,
            [JoinAll] ButtonNode button) {
            UpdateCustomGameTitle(button, lobby, allLobbyUsers.Count);
        }

        [OnEventFire]
        public void UserRemoved(NodeRemoveEvent e, UserNode user, [JoinByBattleLobby] CustomLobbyNode lobby, [JoinByBattleLobby] ICollection<UserNode> allLobbyUsers,
            [JoinAll] ButtonNode button) {
            UpdateCustomGameTitle(button, lobby, allLobbyUsers.Count - 1);
        }

        [OnEventFire]
        public void UpdateOnParamsChanged(NodeAddedEvent e, CustomLobbyNode lobby, [JoinByBattleLobby] ICollection<UserNode> allLobbyUsers, [JoinAll] ButtonNode button) {
            UpdateCustomGameTitle(button, lobby, allLobbyUsers.Count);
        }

        void UpdateCustomGameTitle(ButtonNode button, LobbyNode lobby, int currentPlayersCount) {
            GameModesDescriptionData gameModesDescriptionData = ConfiguratorService.GetConfig("localization/battle_mode").ConvertTo<GameModesDescriptionData>();
            string modeName = gameModesDescriptionData.battleModeLocalization[lobby.battleMode.BattleMode];
            button.playButton.SetCustomModeTitle(modeName, currentPlayersCount, lobby.userLimit.UserLimit);
        }

        [OnEventFire]
        public void SelfUserInSquad(NodeAddedEvent e, SelfUserInSquadNode selfUserInSquad, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void SelfUserOutSquad(NodeRemoveEvent e, SelfUserInSquadNode selfUserInSquad, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void SelfUserIsSquadLeader(NodeAddedEvent e, SelfUserSquadLeader selfUserSquadLeader, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void SelfUserIsNotSquadLeader(NodeRemoveEvent e, SelfUserSquadLeader selfUserSquadLeader, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void ShareEnergyModeOn(NodeAddedEvent e, ShareScreenStateNode shareScreenStateNode, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        [OnEventFire]
        public void ShareEnergyModeOff(NodeRemoveEvent e, ShareScreenStateNode shareScreenStateNode, [JoinAll] ButtonNode button) {
            DelayUpdate(button);
        }

        public class ButtonNode : Node {
            public ESMComponent esm;
            public PlayButtonComponent playButton;
        }

        public class ButtonSearchingStateNode : ButtonNode {
            public PlayerButtonSearchingStateComponent playerButtonSearchingState;
        }

        public class LobbyUINode : Node {
            public MatchLobbyGUIComponent matchLobbyGUI;
        }

        public class LobbyNode : Node {
            public BattleLobbyComponent battleLobby;

            public BattleLobbyGroupComponent battleLobbyGroup;

            public BattleModeComponent battleMode;

            public UserLimitComponent userLimit;
        }

        public class CustomLobbyNode : LobbyNode {
            public CustomBattleLobbyComponent customBattleLobby;
        }

        public class LobbyWithBattleNode : LobbyNode {
            public BattleGroupComponent battleGroup;
        }

        public class SelfLobbyNode : Node {
            public BattleLobbyComponent battleLobby;

            public SelfComponent self;
        }

        public class LobbyWithStartTimeNode : LobbyNode {
            public ClientMatchMakingLobbyStartTimeComponent clientMatchMakingLobbyStartTime;
        }

        public class StartingMMLobbyNode : LobbyNode {
            public ClientMatchMakingLobbyStartingComponent clientMatchMakingLobbyStarting;
        }

        public class StartingCustomLobbyNode : LobbyNode {
            public LobbyStartingStateComponent lobbyStartingState;
        }

        public class UpdatePlayButtonEvent : Event { }

        public class UserNode : Node {
            public BattleLobbyGroupComponent battleLobbyGroup;

            public TeamColorComponent teamColor;
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }

        public class SelfUserInSquadNode : Node {
            public SelfUserComponent selfUser;

            public SquadGroupComponent squadGroup;
        }

        public class SelfUserSquadLeader : SelfUserInSquadNode {
            public SquadLeaderComponent squadLeader;
        }

        public class ShareScreenStateNode : Node {
            public EnergySharingStateComponent energySharingState;
            public SquadComponent squad;
        }
    }
}