using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1430385887057L)]
    public interface BonusRegionAssetsTemplate : Template {
        [PersistentConfig]
        [AutoAdded]
        BonusRegionAssetsComponent bonusRegionAssets();
    }
}