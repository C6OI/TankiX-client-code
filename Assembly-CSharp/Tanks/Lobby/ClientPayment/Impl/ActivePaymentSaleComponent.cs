using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [SerialVersionUID(636605048441969070L)]
    public class ActivePaymentSaleComponent : Component {
        public Date StopTime { get; set; }

        public float PriceMultiplier { get; set; }

        public float AmountMultiplier { get; set; }

        public bool Personal { get; set; }

        [ProtocolTransient] public bool PersonalPriceDiscount => Personal && PriceMultiplier < 1.0;

        [ProtocolTransient] public bool PersonalXCrystalBonus => Personal && AmountMultiplier > 1.0;
    }
}