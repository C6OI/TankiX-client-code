using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(636046254605033322L)]
    public interface WeaponSkinBattleItemTemplate : Template {
        [PersistentConfig("unityAsset")]
        [AutoAdded]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        AssetRequestComponent assetRequest();
    }
}