using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PaymentSpecialIconMinimalRankComponent : LocalizedControl, Component {
        public string MinimalRank { get; set; }
    }
}