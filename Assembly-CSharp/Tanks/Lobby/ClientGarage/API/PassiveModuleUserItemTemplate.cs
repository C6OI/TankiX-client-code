using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1486980355472L)]
    public interface PassiveModuleUserItemTemplate : ModuleUserItemTemplate, ModuleItemTemplate, UserItemTemplate, GarageItemTemplate, Template {
        [AutoAdded]
        PassiveModuleComponent passiveModule();
    }
}