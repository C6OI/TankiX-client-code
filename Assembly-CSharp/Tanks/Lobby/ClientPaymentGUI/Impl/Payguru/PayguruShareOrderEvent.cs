using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl.Payguru {
    [Shared]
    [SerialVersionUID(-1098686186425651898L)]
    public class PayguruShareOrderEvent : Event {
        public string Order { get; set; }

        public PayguruAbbreviatedBankInfo[] BanksInfo { get; set; }
    }
}