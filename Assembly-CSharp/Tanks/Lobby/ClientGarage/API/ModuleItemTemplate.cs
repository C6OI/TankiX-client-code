using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1485165044938L)]
    public interface ModuleItemTemplate : GarageItemTemplate, Template {
        [AutoAdded]
        ModuleItemComponent moduleItem();

        [AutoAdded]
        [PersistentConfig]
        ItemIconComponent itemIcon();

        [AutoAdded]
        [PersistentConfig]
        ItemBigIconComponent itemBigIcon();

        [AutoAdded]
        [PersistentConfig]
        OrderItemComponent orderItem();

        [AutoAdded]
        [PersistentConfig("", true)]
        ModuleEffectsComponent moduleEffects();
    }
}