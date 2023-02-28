using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1545106623033L)]
    public class FractionInvolvedInCompetitionComponent : Component {
        public long UserCount { get; set; }
    }
}