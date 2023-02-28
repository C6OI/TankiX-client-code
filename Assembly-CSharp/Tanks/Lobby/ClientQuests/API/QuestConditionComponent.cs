using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientQuests.API {
    [Shared]
    [SerialVersionUID(1493901546731L)]
    public class QuestConditionComponent : Component {
        public Dictionary<QuestConditionType, long> Condition { get; set; }
    }
}