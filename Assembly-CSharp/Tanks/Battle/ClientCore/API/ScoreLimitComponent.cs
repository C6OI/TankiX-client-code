using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(-3048295118496552479L)]
    public class ScoreLimitComponent : Component {
        public int ScoreLimit { get; set; }
    }
}