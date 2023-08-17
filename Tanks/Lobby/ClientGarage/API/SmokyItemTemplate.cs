using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437710550541L)]
    public interface SmokyItemTemplate : Template, GarageItemTemplate, WeaponItemTemplate {
        [PersistentConfig]
        [AutoAdded]
        MinDamagePropertyComponent minDamageProperty();

        [PersistentConfig]
        [AutoAdded]
        MaxDamagePropertyComponent maxDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        MaxCriticalProbabilityPropertyComponent maxCriticalProbabilityProperty();

        [AutoAdded]
        [PersistentConfig]
        CriticalDamagePropertyComponent criticalDamageProperty();

        [PersistentConfig]
        [AutoAdded]
        CriticalProbabilityDeltaPropertyComponent criticalProbabilityDeltaProperty();

        [AutoAdded]
        [PersistentConfig]
        StartCriticalProbabilityPropertyComponent startCriticalProbabilityProperty();

        [AutoAdded]
        [PersistentConfig]
        ImpactPropertyComponent impactProperty();

        [AutoAdded]
        [PersistentConfig]
        ReloadTimePropertyComponent reloadTimeProperty();

        [AutoAdded]
        [PersistentConfig]
        MinDamageDistancePropertyComponent minDamageDistanceProperty();
    }
}