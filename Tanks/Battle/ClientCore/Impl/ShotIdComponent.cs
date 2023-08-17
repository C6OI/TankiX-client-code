using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(635954739855579490L)]
    public class ShotIdComponent : Component {
        public int ShotId { get; private set; }

        public int NextShotId() => ++ShotId;
    }
}