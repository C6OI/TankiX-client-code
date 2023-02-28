using System;

namespace Tanks.Lobby.ClientQuests.API {
    [Serializable]
    public enum BattleQuestReward {
        CRYSTALS = 0,
        TURRET_EXP = 1,
        HULL_EXP = 2,
        RANK_EXP = 3,
        CHEST_SCORE = 4
    }
}