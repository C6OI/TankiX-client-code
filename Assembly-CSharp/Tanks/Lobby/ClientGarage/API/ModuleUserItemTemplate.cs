using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1484901449548L)]
    public interface ModuleUserItemTemplate : ModuleItemTemplate, UserItemTemplate, GarageItemTemplate, Template { }
}