using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.API {
    [Shared]
    [SerialVersionUID(1495190227237L)]
    public class QuestRarityComponent : Component {
        public QuestRarityType RarityType { get; set; }
    }
}