using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1531929899999L)]
    public interface GoldBonusModuleUserItemTemplate : ModuleUserItemTemplate, ModuleItemTemplate, UserItemTemplate, GarageItemTemplate, Template {
        [AutoAdded]
        GoldBonusModuleItemComponent goldBonusModuleItem();
    }
}