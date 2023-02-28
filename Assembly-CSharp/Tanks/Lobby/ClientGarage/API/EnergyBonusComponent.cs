using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1504269596164L)]
    public class EnergyBonusComponent : Component {
        public int Bonus { get; set; }

        public int PremiumBonus { get; set; }
    }
}