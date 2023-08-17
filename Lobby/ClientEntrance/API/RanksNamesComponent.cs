using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientEntrance.API {
    public class RanksNamesComponent : Component {
        public string[] Names { get; set; }
    }
}