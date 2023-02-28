using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [SerialVersionUID(636173896038335794L)]
    public interface PersonalSpecialOfferPropertiesTemplate : Template {
        [AutoAdded]
        PersonalSpecialOfferPropertiesComponent PersonalSpecialOfferProperties();

        SpecialOfferGroupComponent SpecialOfferGroup();

        SpecialOfferVisibleComponent SpecialOfferVisible();

        SpecialOfferRemainingTimeComponent SpecialOfferRemainingTime();

        SpecialOfferEndTimeComponent specialOfferEndTime();

        UserGroupComponent userGroup();

        [AutoAdded]
        OrderItemComponent orderItem();

        PaymentIntentComponent paymentIntent();
    }
}