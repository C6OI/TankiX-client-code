using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636354483701179398L)]
    public interface ModuleUpgradeBackhitModificatorEffectInfoTemplate : ModuleUpgradeInfoTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ModuleBackhitModificatorEffectPropertyComponent moduleBackhitModificatorEffectProperty();

        [AutoAdded]
        HiddenInGarageItemComponent hiddenInGarageItem();
    }
}