using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1518618376648L)]
    public class MoneyBonusComponent : Component {
        public int Bonus { get; set; }
    }
}