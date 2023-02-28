using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(1949198098578360952L)]
    public class HealthComponent : Component {
        public float CurrentHealth { get; set; }

        public float MaxHealth { get; set; }
    }
}