using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1432033361241L)]
    public interface MineBattleItemTemplate : Template, SupplyBattleItemTemplate {
        MineInventoryComponent mineInventory();
    }
}