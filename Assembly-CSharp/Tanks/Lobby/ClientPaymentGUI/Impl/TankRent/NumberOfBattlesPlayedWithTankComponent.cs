using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientPaymentGUI.Impl.TankRent {
    [Shared]
    [SerialVersionUID(1513257551534L)]
    public class NumberOfBattlesPlayedWithTankComponent : SharedChangeableComponent {
        public int BattlesLeft { get; set; }
    }
}