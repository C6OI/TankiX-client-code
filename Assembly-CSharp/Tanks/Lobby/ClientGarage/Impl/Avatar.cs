using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class Avatar : VisualItem, IAvatarStateChanger, IComparable<Avatar> {
        bool _unlocked = true;

        public Action Remove { get; set; }

        public override Entity MarketItem {
            get => base.MarketItem;
            set {
                base.MarketItem = value;
                IconUid = value.GetComponent<AvatarItemComponent>().Id;
                MinRank = value.GetComponent<PurchaseUserRankRestrictionComponent>().RestrictionValue;
                orderIndex = value.GetComponent<OrderItemComponent>().Index;
            }
        }

        public string RarityName => Rarity.ToString().ToLower();

        public string IconUid { get; private set; }

        public int MinRank { get; private set; }

        public int Index { get; set; }

        int orderIndex { get; set; }

        public bool Unlocked {
            get => _unlocked;
            set {
                _unlocked = value;

                if (SetUnlocked != null) {
                    SetUnlocked(_unlocked);
                }
            }
        }

        public Action<bool> SetSelected { get; set; }

        public Action<bool> SetEquipped { get; set; }

        public Action<bool> SetUnlocked { get; set; }

        public Action OnBought { get; set; }

        public int CompareTo(Avatar other) {
            if (this == other) {
                return 0;
            }

            if (MarketItem.GetComponent<DefaultItemComponent>().Default) {
                return -1;
            }

            if (other.MarketItem.GetComponent<DefaultItemComponent>().Default) {
                return 1;
            }

            if (UserItem != null && other.UserItem == null) {
                return -1;
            }

            if (other.UserItem != null && UserItem == null) {
                return 1;
            }

            if (orderIndex != other.orderIndex) {
                return orderIndex - other.orderIndex;
            }

            if (Rarity != other.Rarity) {
                return other.Rarity - Rarity;
            }

            if (MinRank != other.MinRank) {
                return other.MinRank - MinRank;
            }

            return string.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }
}