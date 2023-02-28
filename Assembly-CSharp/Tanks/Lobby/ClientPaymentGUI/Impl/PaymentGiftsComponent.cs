using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    [Shared]
    [SerialVersionUID(636445619360898464L)]
    public class PaymentGiftsComponent : Component {
        public Dictionary<long, long> Gifts { get; set; }
    }
}