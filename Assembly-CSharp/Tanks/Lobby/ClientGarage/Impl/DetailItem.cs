using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DetailItem : GarageItem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public int Count {
            get {
                if (UserItem == null) {
                    return 0;
                }

                if (!UserItem.HasComponent<UserItemCounterComponent>()) {
                    return 0;
                }

                return (int)UserItem.GetComponent<UserItemCounterComponent>().Count;
            }
        }

        public long RequiredCount => MarketItem.GetComponent<DetailItemComponent>().RequiredCount;

        public override Entity MarketItem {
            get => base.MarketItem;
            set {
                base.MarketItem = value;
                Preview = value.GetComponent<ImageItemComponent>().SpriteUid;
            }
        }

        public GarageItem TargetMarketItem => GarageItemsRegistry.GetItem<GarageItem>(MarketItem.GetComponent<DetailItemComponent>().TargetMarketItemId);

        public override string ToString() => string.Format("Detail item: marketItem = {0}, TargetMarketItem = {1}, Name = {2}, Preview = {3}, Count = {4}, RequiredCount = {5}",
            MarketItem, TargetMarketItem.MarketItem, Name, Preview, Count, RequiredCount);
    }
}