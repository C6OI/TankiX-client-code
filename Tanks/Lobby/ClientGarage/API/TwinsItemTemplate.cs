using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437713726440L)]
    public interface TwinsItemTemplate : Template, GarageItemTemplate, WeaponItemTemplate {
        [AutoAdded]
        [PersistentConfig]
        MinDamagePropertyComponent minDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        MaxDamagePropertyComponent maxDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        ImpactPropertyComponent impactProperty();

        [AutoAdded]
        [PersistentConfig]
        ReloadTimePropertyComponent reloadTimeProperty();

        [AutoAdded]
        [PersistentConfig]
        BulletSpeedPropertyComponent bulletSpeedProperty();

        [AutoAdded]
        [PersistentConfig]
        MinDamageDistancePropertyComponent minDamageDistanceProperty();
    }
}