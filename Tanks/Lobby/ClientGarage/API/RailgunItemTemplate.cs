using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714465485L)]
    public interface RailgunItemTemplate : Template, GarageItemTemplate, WeaponItemTemplate {
        [PersistentConfig]
        [AutoAdded]
        MinDamagePropertyComponent minDamageProperty();

        [PersistentConfig]
        [AutoAdded]
        MaxDamagePropertyComponent maxDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        ChargeTimePropertyComponent chargeTimeProperty();

        [AutoAdded]
        [PersistentConfig]
        DamageWeakeningByTargetPropertyComponent damageWeakeningByTargetProperty();

        [PersistentConfig]
        [AutoAdded]
        ImpactPropertyComponent impactProperty();

        [PersistentConfig]
        [AutoAdded]
        ReloadTimePropertyComponent reloadTimeProperty();
    }
}