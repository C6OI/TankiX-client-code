using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientEntrance.API {
    public class RanksNamesComponent : Component {
        public string[] Names { get; set; }
    }
}