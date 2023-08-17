using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(7115193786389139467L)]
    public class WeaponCooldownComponent : Component {
        public WeaponCooldownComponent() { }

        public WeaponCooldownComponent(float cooldownIntervalSec) => CooldownIntervalSec = cooldownIntervalSec;

        public float CooldownIntervalSec { get; set; }
    }
}