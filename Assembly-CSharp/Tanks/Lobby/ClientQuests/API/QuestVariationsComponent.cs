using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientQuests.API {
    public class QuestVariationsComponent : Component {
        public List<QuestParameters> Quests { get; set; }
    }
}