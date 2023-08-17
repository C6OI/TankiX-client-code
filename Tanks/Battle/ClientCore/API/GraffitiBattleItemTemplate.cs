using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(636100801926133320L)]
    public interface GraffitiBattleItemTemplate : Template {
        [PersistentConfig("unityAsset")]
        [AutoAdded]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        AssetRequestComponent assetRequest();
    }
}