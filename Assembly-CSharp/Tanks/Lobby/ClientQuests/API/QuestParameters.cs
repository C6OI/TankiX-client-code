using System.Collections.Generic;

namespace Tanks.Lobby.ClientQuests.API {
    public class QuestParameters {
        public string Range { get; set; }

        public int TargetValue { get; set; }

        public Dictionary<long, int> QuestReward { get; set; }

        public QuestRarityType QuestRarity { get; set; }

        public List<QuestConditionVariations> QuestConditions { get; set; }

        public float QuestWeight { get; set; }
    }
}