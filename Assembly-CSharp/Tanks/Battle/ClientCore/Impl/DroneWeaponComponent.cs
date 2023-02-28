using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1482462483179L)]
    [Shared]
    public class DroneWeaponComponent : Component {
        public float lastControlTime;
        public float lastTimeTargetSeen;
    }
}