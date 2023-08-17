using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-1730537109851793916L)]
    public interface DamageBattleItemTemplate : Template, SupplyBattleItemTemplate {
        DamageInventoryComponent damageInventory();
    }
}