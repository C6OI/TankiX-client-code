using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(-6551246495632103970L)]
    public class RepairEffectConfigComponent : Component {
        public long TactPeriodMs { get; set; }

        public float HpPerMs { get; set; }

        public float IncrTemperaturePerMs { get; set; }

        public float DecrTemperaturePerMs { get; set; }
    }
}