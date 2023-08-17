using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(6996392210963481981L)]
    public interface ArmorBattleItemTemplate : Template, SupplyBattleItemTemplate {
        ArmorInventoryComponent armorInventory();
    }
}