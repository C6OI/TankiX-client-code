using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class UserInteractionsSystem : ECSSystem {
        [OnEventFire]
        public void ShowTooltipInLobbyRightClick(RightMouseButtonClickEvent e, LobbyInteractableUserButtonNode userButton, [JoinAll] SelfUserNode selfUser,
            [JoinByBattleLobby] LobbyNode lobby) {
            ShowTooltipInLobby(userButton, selfUser, lobby);
        }

        [OnEventFire]
        public void ShowTooltipInLobby(ButtonClickEvent e, LobbyInteractableUserButtonNode userButton, [JoinAll] SelfUserNode selfUser, [JoinByBattleLobby] LobbyNode lobby) {
            ShowTooltipInLobby(userButton, selfUser, lobby);
        }

        void ShowTooltipInLobby(LobbyInteractableUserButtonNode userButton, SelfUserNode selfUser, LobbyNode lobby) {
            if (!userButton.lobbyUserListItem.Empty) {
                long id = userButton.lobbyUserListItem.userEntity.Id;
                GameObject tooltipPrefab = userButton.userInteractionsButton.tooltipPrefab;
                RequestTooltipDisplay(id, tooltipPrefab, selfUser.Entity, InteractionSource.LOBBY, lobby.Entity.Id);
            }
        }

        [OnEventFire]
        public void ShowTooltipInBattleRightClick(RightMouseButtonClickEvent e, BattleInteractableUserButtonNode userButton, [JoinByUser] UserNode user, [JoinAll] SelfUserNode selfUser,
            [JoinByBattle] BattleNode battle) {
            ShowTooltipInBattle(userButton, user, selfUser, battle);
        }

        [OnEventFire]
        public void ShowTooltipInBattle(ButtonClickEvent e, BattleInteractableUserButtonNode userButton, [JoinByUser] UserNode user, [JoinAll] SelfUserNode selfUser,
            [JoinByBattle] BattleNode battle) {
            ShowTooltipInBattle(userButton, user, selfUser, battle);
        }

        void ShowTooltipInBattle(BattleInteractableUserButtonNode userButton, UserNode user, SelfUserNode selfUser, BattleNode battle) {
            if (!selfUser.Entity.HasComponent<UserInBattleAsSpectatorComponent>()) {
                GameObject tooltipPrefab = userButton.userInteractionsButton.tooltipPrefab;
                RequestTooltipDisplay(user.Entity.Id, tooltipPrefab, selfUser.Entity, InteractionSource.BATTLE, battle.Entity.Id);
            }
        }

        [OnEventFire]
        public void DisableSelfInteractionInBattle(NodeAddedEvent e, BattleInteractableUserButtonNode userButton, [JoinByUser] UserNode user, [JoinSelf] SelfUserNode selfUser) {
            userButton.userInteractionsButton.GetComponent<Button>().interactable = false;
        }

        [OnEventFire]
        public void RequestTooltipAtBattleResult(ButtonClickEvent e, SingleNode<UserInteractionsButtonComponent> userButton, [JoinAll] SelfUserNode selfUser) {
            ShowTooltipAtBattleResult(userButton.component, selfUser.Entity);
        }

        [OnEventFire]
        public void RequestTooltipAtBattleResultRightClick(RightMouseButtonClickEvent e, SingleNode<UserInteractionsButtonComponent> userButton, [JoinAll] SelfUserNode selfUser) {
            ShowTooltipAtBattleResult(userButton.component, selfUser.Entity);
        }

        void ShowTooltipAtBattleResult(UserInteractionsButtonComponent userButton, Entity selfUser) {
            PlayerStatInfoUI component = userButton.gameObject.GetComponent<PlayerStatInfoUI>();

            if (component != null) {
                RequestTooltipDisplay(component.userId, userButton.tooltipPrefab, selfUser, InteractionSource.BATTLE_RESULT, component.battleId);
            }
        }

        [OnEventFire]
        public void ShowTooltipOnServerResponse(UserInteractionDataResponseEvent e, SelfUserWithTooltipRequestNode selfUser) {
            long idOfRequestedUser = selfUser.tooltipDataRequest.idOfRequestedUser;
            InteractionSource interactionSource = selfUser.tooltipDataRequest.InteractionSource;
            long interactableSourceId = selfUser.tooltipDataRequest.InteractableSourceId;

            if (e.UserId == idOfRequestedUser) {
                UserInteractionsData userInteractionsData = new();
                userInteractionsData.canRequestFrendship = e.CanRequestFrendship;
                userInteractionsData.friendshipRequestWasSend = e.FriendshipRequestWasSend;
                userInteractionsData.isMuted = e.Muted;
                userInteractionsData.isReported = e.Reported;
                userInteractionsData.selfUserEntity = selfUser.Entity;
                userInteractionsData.userId = e.UserId;
                userInteractionsData.interactionSource = interactionSource;
                userInteractionsData.sourceId = interactableSourceId;
                userInteractionsData.OtherUserName = e.UserUid;
                UserInteractionsData data = userInteractionsData;
                ShowTooltop(data, selfUser.tooltipDataRequest.TooltipPrefab, selfUser.tooltipDataRequest.MousePosition);
            }
        }

        [OnEventFire]
        public void HideTooltipWhenScreenChanged(ChangeScreenEvent e, Node any, [JoinAll] SelfUserNode selfUser) {
            selfUser.Entity.RemoveComponentIfPresent<TooltipDataRequestComponent>();
            TooltipController.Instance.HideTooltip();
        }

        void RequestTooltipDisplay(long interactableUserId, GameObject tooltipPrefab, Entity selfUser, InteractionSource interactionSource, long sourceId) {
            if (interactableUserId != selfUser.Id) {
                AddTooltipRequestComponent(selfUser, tooltipPrefab, interactableUserId, interactionSource, sourceId);

                ScheduleEvent(new UserInteractionDataRequestEvent {
                    UserId = interactableUserId
                }, selfUser);
            }
        }

        void AddTooltipRequestComponent(Entity entity, GameObject tooltipPrefab, long idOfRequestedUser, InteractionSource interactionSource, long sourceId) {
            TooltipDataRequestComponent tooltipDataRequestComponent = !entity.HasComponent<TooltipDataRequestComponent>()
                                                                          ? entity.AddComponentAndGetInstance<TooltipDataRequestComponent>()
                                                                          : entity.GetComponent<TooltipDataRequestComponent>();

            tooltipDataRequestComponent.MousePosition = Input.mousePosition;
            tooltipDataRequestComponent.TooltipPrefab = tooltipPrefab;
            tooltipDataRequestComponent.InteractionSource = interactionSource;
            tooltipDataRequestComponent.idOfRequestedUser = idOfRequestedUser;
            tooltipDataRequestComponent.InteractableSourceId = sourceId;
        }

        void ShowTooltop(UserInteractionsData data, GameObject tooltipPrefab, Vector3 position) {
            TooltipController.Instance.ShowTooltip(position, data, tooltipPrefab, false);
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserUidComponent userUid;
        }

        public class LobbyInteractableUserButtonNode : Node {
            public LobbyUserListItemComponent lobbyUserListItem;
            public UserInteractionsButtonComponent userInteractionsButton;
        }

        public class BattleInteractableUserButtonNode : Node {
            public UserGroupComponent userGroup;
            public UserInteractionsButtonComponent userInteractionsButton;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
        }

        public class SelfUserWithTooltipRequestNode : SelfUserNode {
            public TooltipDataRequestComponent tooltipDataRequest;
        }

        public class LobbyNode : Node {
            public BattleLobbyComponent battleLobby;
        }

        public class BattleNode : Node {
            public BattleComponent battle;
        }
    }
}