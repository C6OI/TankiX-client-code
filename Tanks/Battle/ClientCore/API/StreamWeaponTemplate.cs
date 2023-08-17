using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1430285569243L)]
    public interface StreamWeaponTemplate : Template, WeaponTemplate {
        StreamWeaponControllerComponent streamWeaponController();

        StreamWeaponEnergyComponent streamWeaponEnergy();

        [PersistentConfig]
        ConicTargetingComponent conicTargeting();

        [AutoAdded]
        EnergyBarComponent energyBar();
    }
}