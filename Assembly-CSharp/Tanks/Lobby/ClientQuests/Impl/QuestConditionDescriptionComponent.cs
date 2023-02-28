using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientQuests.API;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class QuestConditionDescriptionComponent : Component {
        public ConditionDescription condition { get; set; }

        public string restrictionFormat { get; set; }

        public Dictionary<QuestConditionType, string> restrictions { get; set; }
    }
}