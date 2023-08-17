using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714683461L)]
    public interface FreezeItemTemplete : Template, GarageItemTemplate, WeaponItemTemplate {
        [AutoAdded]
        [PersistentConfig]
        DamagePerSecondPropertyComponent damagePerSecondProperty();

        [PersistentConfig]
        [AutoAdded]
        EnergyChargeSpeedPropertyComponent energyChargeSpeedProperty();

        [PersistentConfig]
        [AutoAdded]
        EnergyRechargeSpeedPropertyComponent energyRechargeSpeedProperty();

        [PersistentConfig]
        [AutoAdded]
        MinDamageDistancePropertyComponent minDamageDistanceProperty();
    }
}