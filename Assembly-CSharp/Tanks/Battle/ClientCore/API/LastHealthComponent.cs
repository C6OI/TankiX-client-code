using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class LastHealthComponent : Component {
        public float LastHealth { get; set; }
    }
}