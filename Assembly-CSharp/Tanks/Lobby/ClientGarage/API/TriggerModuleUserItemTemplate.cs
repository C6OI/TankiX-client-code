using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(636304361927229412L)]
    public interface TriggerModuleUserItemTemplate : PassiveModuleUserItemTemplate, ModuleUserItemTemplate, ModuleItemTemplate, UserItemTemplate, GarageItemTemplate, Template { }
}