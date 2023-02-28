using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientFriends.API {
    public class FriendInteractionTooltipData {
        public Entity FriendEntity { get; set; }

        public bool ShowRemoveButton { get; set; }

        public bool ShowEnterAsSpectatorButton { get; set; }

        public bool ShowInviteToSquadButton { get; set; }

        public bool ActiveShowInviteToSquadButton { get; set; }

        public bool ShowRequestToSquadButton { get; set; }

        public bool ShowChatButton { get; set; }
    }
}