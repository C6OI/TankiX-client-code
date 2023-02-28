using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PurchasedItemListComponent : Component {
        readonly List<long> purchasedItems = new();

        public void AddPurchasedItem(long marketItemId) {
            purchasedItems.Add(marketItemId);
        }

        public bool Contains(long marketItemId) => purchasedItems.Contains(marketItemId);
    }
}