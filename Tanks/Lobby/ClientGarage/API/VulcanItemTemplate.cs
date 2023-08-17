using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437714765888L)]
    public interface VulcanItemTemplate : Template, GarageItemTemplate, WeaponItemTemplate {
        [PersistentConfig]
        [AutoAdded]
        DamagePerSecondPropertyComponent damagePerSecondProperty();

        [AutoAdded]
        [PersistentConfig]
        SpinUpTimePropertyComponent spinUpTimeProperty();

        [PersistentConfig]
        [AutoAdded]
        TemperatureLimitPropertyComponent temperatureLimitProperty();

        [AutoAdded]
        [PersistentConfig]
        TemperatureHittingTimePropertyComponent temperatureHittingTimeProperty();

        [PersistentConfig]
        [AutoAdded]
        ImpactPropertyComponent impactProperty();

        [AutoAdded]
        [PersistentConfig]
        MinDamageDistancePropertyComponent minDamageDistanceProperty();
    }
}