using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientFriends.API {
    public class CheckForShowInviteToSquadEvent : Event {
        public bool ShowInviteToSquadButton { get; set; }

        public bool ActiveInviteToSquadButton { get; set; }

        public bool ShowRequestToInviteToSquadButton { get; set; }
    }
}