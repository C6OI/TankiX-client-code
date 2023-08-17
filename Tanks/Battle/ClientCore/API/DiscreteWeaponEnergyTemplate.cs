using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1430285966754L)]
    public interface DiscreteWeaponEnergyTemplate : Template, DiscreteWeaponTemplate, WeaponTemplate { }
}