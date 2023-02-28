using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.API {
    [Shared]
    [SerialVersionUID(1494535525136L)]
    public class SlotIndexComponent : Component {
        public int Index { get; set; }
    }
}