using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-5342270968507348251L)]
    public interface ShellBattleItemTemplate : Template {
        [AutoAdded]
        [PersistentConfig("unityAsset")]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        AssetRequestComponent assetRequest();
    }
}