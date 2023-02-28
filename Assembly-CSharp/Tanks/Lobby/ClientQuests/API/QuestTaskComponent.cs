using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.API {
    [Shared]
    [SerialVersionUID(1493197400931L)]
    public class QuestTaskComponent : Component {
        public int TaskValue { get; set; }
    }
}