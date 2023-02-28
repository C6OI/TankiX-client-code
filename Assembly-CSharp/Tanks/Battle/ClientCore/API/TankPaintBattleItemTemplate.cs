using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1437375358285L)]
    public interface TankPaintBattleItemTemplate : Template {
        [AutoAdded]
        [PersistentConfig("unityAsset")]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        AssetRequestComponent assetRequest();
    }
}