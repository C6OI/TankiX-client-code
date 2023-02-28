using System;
using Tanks.Lobby.ClientQuests.API;
using UnityEngine;

namespace Tanks.Lobby.ClientQuests.Impl {
    public class InBattleQuestItemGUIRewardContainerComponent : MonoBehaviour {
        [SerializeField] InBattleQuestItemGUIRewardComponent itemReward;

        [SerializeField] InBattleQuestItemGUIRewardComponent experienceReward;

        [SerializeField] InBattleQuestItemGUIRewardComponent crystalReward;

        public void SetActiveReward(BattleQuestReward reward, int quantity, long itemId) {
            itemReward.gameObject.SetActive(false);
            experienceReward.gameObject.SetActive(false);
            crystalReward.gameObject.SetActive(false);

            switch (reward) {
                case BattleQuestReward.CRYSTALS:
                    crystalReward.gameObject.SetActive(true);
                    crystalReward.SetReward(quantity);
                    break;

                case BattleQuestReward.TURRET_EXP:
                case BattleQuestReward.HULL_EXP:
                    itemReward.gameObject.SetActive(true);
                    itemReward.SetReward(quantity, itemId);
                    break;

                case BattleQuestReward.RANK_EXP:
                    experienceReward.gameObject.SetActive(true);
                    experienceReward.SetReward(quantity);
                    break;

                case BattleQuestReward.CHEST_SCORE:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("reward");
            }
        }
    }
}