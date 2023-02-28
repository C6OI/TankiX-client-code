using System.Collections.Generic;

namespace Tanks.Lobby.ClientQuests.API {
    public class QuestConditionVariations {
        public QuestConditionType Type { get; set; }

        public List<long> items { get; set; }
    }
}