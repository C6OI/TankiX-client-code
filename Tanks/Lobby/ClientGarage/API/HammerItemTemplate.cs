using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714587857L)]
    public interface HammerItemTemplate : Template, GarageItemTemplate, WeaponItemTemplate {
        [PersistentConfig]
        [AutoAdded]
        DamagePerPelletPropertyComponent damagePerPelletProperty();

        [AutoAdded]
        [PersistentConfig]
        ReloadMagazineTimePropertyComponent reloadMagazineTimeProperty();

        [AutoAdded]
        [PersistentConfig]
        MagazineSizePropertyComponent magazineSizeProperty();

        [AutoAdded]
        [PersistentConfig]
        ImpactPropertyComponent impactProperty();

        [AutoAdded]
        [PersistentConfig]
        ReloadTimePropertyComponent reloadTimeProperty();

        [PersistentConfig]
        [AutoAdded]
        MinDamageDistancePropertyComponent minDamageDistanceProperty();
    }
}