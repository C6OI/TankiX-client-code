using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(8236491228938594733L)]
    [Shared]
    public class WeaponEnergyComponent : Component {
        public WeaponEnergyComponent() { }

        public WeaponEnergyComponent(float energy) => Energy = energy;

        public float Energy { get; set; }
    }
}