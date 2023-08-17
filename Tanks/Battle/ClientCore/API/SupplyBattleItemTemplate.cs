using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-1149044404954795873L)]
    public interface SupplyBattleItemTemplate : Template {
        SupplyBattleItemComponent supplyBattleItem();

        UserGroupComponent userJoin();

        InventoryItemEnabledStateComponent inventoryItemEnabledState();

        InventoryItemDisabledStateComponent inventoryItemDisabledState();

        InventoryItemCooldownTimeComponent inventoryItemCooldownTime();

        InventoryItemActivatedStateComponent inventoryItemActivatedState();
    }
}