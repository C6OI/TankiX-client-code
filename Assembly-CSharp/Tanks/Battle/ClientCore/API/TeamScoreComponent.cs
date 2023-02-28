using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-2440064891528955383L)]
    public class TeamScoreComponent : Component {
        public int Score { get; set; }
    }
}