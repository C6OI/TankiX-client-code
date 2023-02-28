using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1435032375010L)]
    public interface GarageTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        ItemUpgradeExperiencesConfigComponent upgradeLevels();

        [AutoAdded]
        [PersistentConfig]
        SlotsTextsComponent slotsTexts();

        [AutoAdded]
        [PersistentConfig]
        ModuleTypesImagesComponent moduleTypesImages();

        [AutoAdded]
        [PersistentConfig]
        LocalizedVisualPropertiesComponent localizedVisualProperties();
    }
}