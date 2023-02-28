using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1487227856805L)]
    [Shared]
    public class SpiderMineConfigComponent : Component {
        public float Speed { get; set; }

        public float Acceleration { get; set; }

        public float Energy { get; set; }

        public float IdleEnergyDrainRate { get; set; }

        public float ChasingEnergyDrainRate { get; set; }
    }
}