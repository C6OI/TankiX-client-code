using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.API {
    [Shared]
    [SerialVersionUID(1478858859770L)]
    public class KillsInManyBattlesQuestComponent : Component {
        public int Battles { get; set; }
    }
}