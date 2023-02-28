using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class GoodsSelectionScreenComponent : LocalizedScreenComponent, PaymentScreen {
        public string SpecialOfferOneShotMessage { get; set; }

        public string SpecialOfferEmptyRewardMessage { get; set; }

        public XCrystalsDataProvider XCrystalsDataProvider { get; private set; }

        public SpecialOfferDataProvider SpecialOfferDataProvider { get; private set; }

        protected override void Awake() {
            base.Awake();
            XCrystalsDataProvider = GetComponentInChildren<XCrystalsDataProvider>();
            SpecialOfferDataProvider = GetComponentInChildren<SpecialOfferDataProvider>();
        }
    }
}