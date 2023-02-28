using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CustomizationUISystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void InitHull(NodeAddedEvent e, SingleNode<CustomizationUIComponent> ui, HullNode hull, [Context] [JoinByMarketItem] MarketItemNode marketItem) {
            ui.component.Hull = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
        }

        [OnEventFire]
        public void InitTurret(NodeAddedEvent e, SingleNode<CustomizationUIComponent> ui, TurretNode turret, [Context] [JoinByMarketItem] MarketItemNode marketItem) {
            ui.component.Turret = GarageItemsRegistry.GetItem<TankPartItem>(marketItem.Entity);
        }

        public class HullNode : Node {
            public HangarItemPreviewComponent hangarItemPreview;

            public MarketItemGroupComponent marketItemGroup;

            public TankItemComponent tankItem;
        }

        public class TurretNode : Node {
            public HangarItemPreviewComponent hangarItemPreview;

            public MarketItemGroupComponent marketItemGroup;

            public WeaponItemComponent weaponItem;
        }

        public class MarketItemNode : Node {
            public MarketItemComponent marketItem;
            public MarketItemGroupComponent marketItemGroup;
        }
    }
}