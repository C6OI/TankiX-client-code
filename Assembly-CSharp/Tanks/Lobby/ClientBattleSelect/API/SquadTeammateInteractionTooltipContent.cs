using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class SquadTeammateInteractionTooltipContent : InteractionTooltipContent {
        [SerializeField] Button profileButton;

        [SerializeField] Button leaveSquadButton;

        [SerializeField] Button removeFromSquadButton;

        [SerializeField] Button giveLeaderButton;

        [SerializeField] Button addFriendButton;

        [SerializeField] Button friendRequestSentButton;

        [SerializeField] Button changeAvatarButton;

        public LocalizedField friendRequestResponce;

        Entity teammateEntity;

        protected override void Awake() {
            base.Awake();
            profileButton.onClick.AddListener(OpenProfile);
            leaveSquadButton.onClick.AddListener(LeaveSquad);
            removeFromSquadButton.onClick.AddListener(RemoveFromSquad);
            giveLeaderButton.onClick.AddListener(GiveLeader);
            addFriendButton.onClick.AddListener(AddToFriendsList);
            changeAvatarButton.onClick.AddListener(ShowAvatarMenu);
        }

        public override void Init(object data) {
            SquadTeammateInteractionTooltipContentData squadTeammateInteractionTooltipContentData = (SquadTeammateInteractionTooltipContentData)data;
            teammateEntity = squadTeammateInteractionTooltipContentData.teammateEntity;
            changeAvatarButton.gameObject.SetActive(!squadTeammateInteractionTooltipContentData.ShowProfileButton);
            profileButton.gameObject.SetActive(squadTeammateInteractionTooltipContentData.ShowProfileButton);
            leaveSquadButton.gameObject.SetActive(squadTeammateInteractionTooltipContentData.ShowLeaveSquadButton);
            removeFromSquadButton.gameObject.SetActive(squadTeammateInteractionTooltipContentData.ShowRemoveFromSquadButton);
            removeFromSquadButton.interactable = squadTeammateInteractionTooltipContentData.ActiveRemoveFromSquadButton;
            giveLeaderButton.gameObject.SetActive(squadTeammateInteractionTooltipContentData.ShowGiveLeaderButton);
            giveLeaderButton.interactable = squadTeammateInteractionTooltipContentData.ActiveGiveLeaderButton;
            addFriendButton.gameObject.SetActive(squadTeammateInteractionTooltipContentData.ShowAddFriendButton);
            friendRequestSentButton.gameObject.SetActive(squadTeammateInteractionTooltipContentData.ShowFriendRequestSentButton);
        }

        public void OpenProfile() {
            if (teammateEntity != null) {
                EngineService.Engine.ScheduleEvent(new ShowProfileScreenEvent(teammateEntity.Id), teammateEntity);
            }

            Hide();
        }

        public void LeaveSquad() {
            if (teammateEntity != null) {
                EngineService.Engine.ScheduleEvent<LeaveSquadInternalEvent>(teammateEntity);
            }

            Hide();
        }

        public void RemoveFromSquad() {
            if (teammateEntity != null) {
                EngineService.Engine.ScheduleEvent<KickOutFromSquadInternalEvent>(teammateEntity);
            }

            Hide();
        }

        public void GiveLeader() {
            if (teammateEntity != null) {
                EngineService.Engine.ScheduleEvent<ChangeSquadLeaderInternalEvent>(teammateEntity);
            }

            Hide();
        }

        public void AddToFriendsList() {
            if (teammateEntity != null) {
                EngineService.Engine.ScheduleEvent<RequestFriendSquadInternalEvent>(teammateEntity);
                ShowResponse(friendRequestResponce.Value);
            }

            Hide();
        }

        public void ShowAvatarMenu() {
            EngineService.Engine.ScheduleEvent<AvatarMenuSystem.ShowMenuEvent>(new EntityStub());
            Hide();
        }
    }
}