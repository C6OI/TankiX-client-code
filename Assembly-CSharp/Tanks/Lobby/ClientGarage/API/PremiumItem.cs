using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class PremiumItem : VisualItem {
        [Inject] public new static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public new TankPartItem ParentItem { get; private set; }

        public override Entity MarketItem {
            get => base.MarketItem;
            set {
                base.MarketItem = value;
                Preview = value.GetComponent<CardImageItemComponent>().SpriteUid;
                Type = VisualItemType.Other;
            }
        }
    }
}