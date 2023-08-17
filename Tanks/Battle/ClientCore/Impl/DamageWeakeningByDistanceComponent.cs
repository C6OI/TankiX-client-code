using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(2869455602943064305L)]
    [Shared]
    public class DamageWeakeningByDistanceComponent : Component {
        public float RadiusOfMaxDamage { get; set; }

        public float RadiusOfMinDamage { get; set; }

        public float MinDamagePercent { get; set; }
    }
}