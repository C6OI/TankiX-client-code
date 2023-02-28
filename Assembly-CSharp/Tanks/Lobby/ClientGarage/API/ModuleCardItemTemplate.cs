using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636319305482344832L)]
    public interface ModuleCardItemTemplate : GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate, Template {
        [AutoAdded]
        ModuleCardItemComponent moduleCardItem();
    }
}