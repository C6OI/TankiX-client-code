using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437713786006L)]
    public interface ThunderItemTemplate : Template, GarageItemTemplate, WeaponItemTemplate {
        [PersistentConfig]
        [AutoAdded]
        MinDamagePropertyComponent minDamageProperty();

        [PersistentConfig]
        [AutoAdded]
        MaxDamagePropertyComponent maxDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        ImpactPropertyComponent impactProperty();

        [PersistentConfig]
        [AutoAdded]
        ReloadTimePropertyComponent reloadTimeProperty();

        [PersistentConfig]
        [AutoAdded]
        MinDamageDistancePropertyComponent minDamageDistanceProperty();
    }
}