using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(2587842501244166778L)]
    public interface SpeedBattleItemTemplate : Template, SupplyBattleItemTemplate {
        SpeedInventoryComponent speedInventory();
    }
}