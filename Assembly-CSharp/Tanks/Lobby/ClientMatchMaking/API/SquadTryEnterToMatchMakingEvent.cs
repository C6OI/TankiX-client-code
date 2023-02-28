using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientMatchMaking.API {
    [Shared]
    [SerialVersionUID(1510144894187L)]
    public class SquadTryEnterToMatchMakingEvent : Event {
        public long MatchMakingModeId { get; set; }

        public bool RatingMatchMakingMode { get; set; }
    }
}