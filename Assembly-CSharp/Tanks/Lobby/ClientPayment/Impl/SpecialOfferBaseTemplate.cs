using Lobby.ClientPayment.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [SerialVersionUID(657658796589123211L)]
    public interface SpecialOfferBaseTemplate : GoodsTemplate, Template {
        [AutoAdded]
        SpecialOfferComponent specialOffer();

        SpecialOfferGroupComponent specialOfferGroup();

        ItemsPackFromConfigComponent itemsPackFromConfig();

        XCrystalsPackComponent xCrystalsPack();

        SpecialOfferDurationComponent specialOfferDuration();

        SpecialOfferEndTimeComponent specialOfferEndTime();

        [AutoAdded]
        [PersistentConfig("order")]
        OrderItemComponent orderItem();

        [AutoAdded]
        [PersistentConfig]
        SpecialOfferContentLocalizationComponent specialOfferContentLocalization();

        [AutoAdded]
        [PersistentConfig]
        SpecialOfferContentComponent specialOfferContent();

        [AutoAdded]
        [PersistentConfig]
        ReceiptTextComponent receiptText();

        [AutoAdded]
        [PersistentConfig]
        SpecialOfferScreenLocalizationComponent specialOfferScreenLocalization();

        CountableItemsPackComponent countableItemsPack();

        CrystalsPackComponent crystalsPack();
    }
}