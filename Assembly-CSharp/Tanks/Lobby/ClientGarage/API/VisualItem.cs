using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Lobby.ClientGarage.API {
    public class VisualItem : GarageItem {
        public enum VisualItemType {
            Skin = 0,
            Paint = 1,
            Coating = 2,
            Graffiti = 3,
            Shell = 4,
            Other = 5,
            Avatar = 6
        }

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public TankPartItem ParentItem { get; private set; }

        public override Entity MarketItem {
            get => base.MarketItem;
            set {
                base.MarketItem = value;
                Preview = value.GetComponent<ImageItemComponent>().SpriteUid;

                if (value.HasComponent<SkinItemComponent>()) {
                    Type = VisualItemType.Skin;
                } else if (value.HasComponent<GraffitiItemComponent>()) {
                    Type = VisualItemType.Graffiti;
                } else if (value.HasComponent<TankPaintItemComponent>()) {
                    Type = VisualItemType.Paint;
                } else if (value.HasComponent<WeaponPaintItemComponent>()) {
                    Type = VisualItemType.Coating;
                } else if (value.HasComponent<ShellItemComponent>()) {
                    Type = VisualItemType.Shell;
                } else if (value.HasComponent<AvatarItemComponent>()) {
                    Type = VisualItemType.Avatar;
                } else {
                    Type = VisualItemType.Other;
                    Preview = value.GetComponent<CardImageItemComponent>().SpriteUid;
                }

                if (MarketItem.HasComponent<ParentGroupComponent>()) {
                    ParentItem = GarageItemsRegistry.GetItem<TankPartItem>(MarketItem.GetComponent<ParentGroupComponent>().Key);
                }
            }
        }

        public VisualItemType Type { get; protected set; }
    }
}