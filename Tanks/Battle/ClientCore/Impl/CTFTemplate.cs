using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(-1911920453295891173L)]
    public interface CTFTemplate : Template, BattleTemplate, TeamBattleTemplate {
        [PersistentConfig("unityAsset")]
        AssetReferenceComponent assetReference();

        AssetRequestComponent assetRequest();

        CTFComponent ctfComponent();

        [AutoAdded]
        [PersistentConfig("ctfConfig")]
        CTFConfigComponent CTFConfig();
    }
}