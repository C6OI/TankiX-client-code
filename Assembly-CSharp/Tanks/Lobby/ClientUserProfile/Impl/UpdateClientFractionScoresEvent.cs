using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1545371971895L)]
    public class UpdateClientFractionScoresEvent : Event {
        public long TotalCryFund { get; set; }

        public Dictionary<long, long> Scores { get; set; }
    }
}