using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientFriends.API {
    public class FriendInteractionTooltipContentComponent : InteractionTooltipContent, AttachToEntityListener {
        [SerializeField] Button profileButton;

        [SerializeField] Button chatButton;

        [SerializeField] Button enterAsSpectatorButton;

        [SerializeField] Button removeButton;

        [SerializeField] Button inviteToSquadButton;

        [SerializeField] Button requestToSquadButton;

        [SerializeField] Button requestToSquadWasSentButton;

        [SerializeField] Button squadIsFullButton;

        public LocalizedField inviteToSquadResponce;

        public LocalizedField requestToSquadResponce;

        Entity friendEntity;

        protected override void Awake() {
            base.Awake();
            profileButton.onClick.AddListener(OpenProfile);
            chatButton.onClick.AddListener(StartChat);
            removeButton.onClick.AddListener(RemoveFriend);
            enterAsSpectatorButton.onClick.AddListener(EnterAsSpectator);
            inviteToSquadButton.onClick.AddListener(InviteToSquad);
            requestToSquadButton.onClick.AddListener(RequestForInviteToSquad);
        }

        public void AttachedToEntity(Entity entity) {
            if (friendEntity != null) {
                friendEntity.GetComponent<UserGroupComponent>().Attach(GetComponent<EntityBehaviour>().Entity);
            }
        }

        public override void Init(object data) {
            FriendInteractionTooltipData friendInteractionTooltipData = (FriendInteractionTooltipData)data;
            friendEntity = friendInteractionTooltipData.FriendEntity;
            removeButton.gameObject.SetActive(friendInteractionTooltipData.ShowRemoveButton);
            enterAsSpectatorButton.gameObject.SetActive(friendInteractionTooltipData.ShowEnterAsSpectatorButton);
            inviteToSquadButton.gameObject.SetActive(friendInteractionTooltipData.ShowInviteToSquadButton);
            inviteToSquadButton.interactable = friendInteractionTooltipData.ActiveShowInviteToSquadButton;
            requestToSquadButton.gameObject.SetActive(friendInteractionTooltipData.ShowRequestToSquadButton);
            chatButton.gameObject.SetActive(friendInteractionTooltipData.ShowChatButton);
        }

        public void OpenProfile() {
            if (friendEntity != null) {
                EngineService.Engine.ScheduleEvent(new ShowProfileScreenEvent(friendEntity.Id), friendEntity);
            }

            Hide();
        }

        public void StartChat() {
            if (friendEntity != null) {
                EngineService.Engine.ScheduleEvent(new OpenPersonalChatFromContextMenuEvent(), friendEntity);
            }

            Hide();
        }

        public void InviteToSquad() {
            if (friendEntity != null) {
                EngineService.Engine.ScheduleEvent(new FriendInviteToSquadEvent(friendEntity.Id, InteractionSource.FRIENDS_LIST, 0L), friendEntity);
                ShowResponse(inviteToSquadResponce.Value);
            }

            Hide();
        }

        public void RequestForInviteToSquad() {
            if (friendEntity != null) {
                RequestToSquadWasSent();
                EngineService.Engine.ScheduleEvent<RequestToSquadInternalEvent>(friendEntity);
            }

            Hide();
        }

        public void RemoveFriend() {
            if (friendEntity != null) {
                EngineService.Engine.ScheduleEvent<RemoveFriendButtonEvent>(friendEntity);
            }

            Hide();
        }

        public void EnterAsSpectator() {
            if (friendEntity != null) {
                EngineService.Engine.ScheduleEvent<EnterAsSpectatorToFriendBattleEvent>(friendEntity);
            }

            Hide();
        }

        public void RequestToSquadWasSent() {
            requestToSquadButton.gameObject.SetActive(false);
            requestToSquadWasSentButton.gameObject.SetActive(true);
        }

        public void SquadIsFull() {
            if (requestToSquadButton.gameObject.activeInHierarchy || requestToSquadWasSentButton.gameObject.activeInHierarchy) {
                requestToSquadButton.gameObject.SetActive(false);
                requestToSquadWasSentButton.gameObject.SetActive(false);
                squadIsFullButton.gameObject.SetActive(true);
            }
        }
    }
}