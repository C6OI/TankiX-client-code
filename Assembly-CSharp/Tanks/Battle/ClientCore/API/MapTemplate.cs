using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientResources.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-5630755063511713066L)]
    public interface MapTemplate : Template {
        MapComponent map();

        [AutoAdded]
        [PersistentConfig]
        DescriptionItemComponent descriptionItem();

        [AutoAdded]
        [PersistentConfig]
        AssetReferenceComponent assetReference();

        [AutoAdded]
        [PersistentConfig]
        MapPreviewComponent mapPreview();

        [AutoAdded]
        [PersistentConfig]
        MapLoadPreviewComponent mapLoadPreview();

        [AutoAdded]
        [PersistentConfig]
        FlavorListComponent flavorList();

        AssetRequestComponent assetRequest();

        MapInstanceComponent mapInstance();

        [AutoAdded]
        [PersistentConfig]
        MapModeRestrictionComponent mapModeRestriction();
    }
}