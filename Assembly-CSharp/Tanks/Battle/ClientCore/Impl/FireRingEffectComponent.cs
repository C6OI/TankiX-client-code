using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1542695311337L)]
    public class FireRingEffectComponent : Component {
        public long Duration { get; set; }

        public float TemperatureDelta { get; set; }

        public float TemperatureLimit { get; set; }
    }
}