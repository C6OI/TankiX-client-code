using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1437484854932L)]
    public interface SupplyUserItemTemplate : Template, GarageItemImagedTemplate, GarageItemTemplate, ItemImagedTemplate,
        UserItemTemplate {
        [PersistentConfig]
        [AutoAdded]
        SupplyPropertyTemplateComponent supplyPropertyTemplate();

        SupplyCountComponent supplyCount();

        SupplyTypeComponent supplyType();

        CooldownConfigComponent cooldownConfig();

        SupplyGroupComponent supplyGroup();
    }
}