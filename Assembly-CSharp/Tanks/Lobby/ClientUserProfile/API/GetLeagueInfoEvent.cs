using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1522323975002L)]
    public class GetLeagueInfoEvent : Event {
        public long UserId { get; set; }
    }
}