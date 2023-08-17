using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-6600512566895926352L)]
    public interface RepairBattleItemTemplate : Template, SupplyBattleItemTemplate {
        RepairInventoryComponent repairInventory();
    }
}