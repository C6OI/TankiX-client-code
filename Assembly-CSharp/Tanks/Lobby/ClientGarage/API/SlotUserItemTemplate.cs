using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1485846188251L)]
    public interface SlotUserItemTemplate : UserItemTemplate, Template {
        [AutoAdded]
        SlotItemComponent slotItem();
    }
}