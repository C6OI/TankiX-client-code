using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(1481612319098L)]
    public class XCrystalsAmountSaleComponent : Component {
        public Date StartTime { get; set; }

        public Date StopTime { get; set; }

        public double Multiplier { get; set; }

        public long Addition { get; set; }
    }
}