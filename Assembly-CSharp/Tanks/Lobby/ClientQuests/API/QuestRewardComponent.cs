using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.API {
    [Shared]
    [SerialVersionUID(1493196614850L)]
    public class QuestRewardComponent : Component {
        public Dictionary<long, int> Reward { get; set; }
    }
}