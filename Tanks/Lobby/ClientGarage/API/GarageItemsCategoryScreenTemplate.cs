using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1444301436663L)]
    public interface GarageItemsCategoryScreenTemplate : ScreenTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        GarageItemsCategoryLocalizedStringsComponent garageItemsCategoryLocalizedStringsComponent();
    }
}