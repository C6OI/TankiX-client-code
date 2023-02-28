using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1545394828752L)]
    public class FinishedFractionsCompetitionComponent : Component {
        public Entity Winner { get; set; }
    }
}