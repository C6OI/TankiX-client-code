using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class TemperatureConfigComponent : Component {
        public float MaxTemperature { get; set; }

        public float MinTemperature { get; set; }

        public float AutoIncrementInMs { get; set; }

        public float AutoDecrementInMs { get; set; }

        public int TactPeriodInMs { get; set; }

        public float MinFlameDamage { get; set; }

        public float MaxFlameDamage { get; set; }
    }
}