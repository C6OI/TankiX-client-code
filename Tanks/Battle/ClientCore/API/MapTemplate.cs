using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-5630755063511713066L)]
    public interface MapTemplate : Template {
        MapComponent map();

        [PersistentConfig]
        [AutoAdded]
        MapNameComponent mapName();

        [PersistentConfig]
        [AutoAdded]
        AssetReferenceComponent assetReference();

        [PersistentConfig]
        [AutoAdded]
        MapPreviewComponent mapPreview();

        AssetRequestComponent assetRequest();

        MapInstanceComponent mapInstance();
    }
}