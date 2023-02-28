using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1543484139511L)]
    public interface ModuleUpgradeExplosiveMassEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        ModuleExplosiveMassEffectPropertyComponent moduleExplosiveMassEffectProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectMaxDamagePropertyComponent moduleEffectMaxDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectMinDamagePropertyComponent moduleEffectMinDamageProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectTargetingDistancePropertyComponent moduleEffectTargetingDistanceProperty();

        [AutoAdded]
        [PersistentConfig]
        ModuleEffectActivationTimePropertyComponent moduleEffectActivationTimeProperty();
    }
}