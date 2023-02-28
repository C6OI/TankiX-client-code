using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientMatchMaking.API {
    [Shared]
    [SerialVersionUID(1511250192647L)]
    public class SquadTryEnterToMatchMakingAfterEnergySharingEvent : Event {
        public long MatchMakingModeId { get; set; }
    }
}