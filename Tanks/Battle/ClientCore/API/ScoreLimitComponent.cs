using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-3048295118496552479L)]
    [Shared]
    public class ScoreLimitComponent : Component {
        public int ScoreLimit { get; set; }
    }
}