using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1544590059379L)]
    public class FractionUserScoreComponent : Component {
        public long TotalEarnedPoints { get; set; }
    }
}