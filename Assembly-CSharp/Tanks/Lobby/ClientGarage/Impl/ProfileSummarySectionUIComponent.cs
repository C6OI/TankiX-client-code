using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ProfileSummarySectionUIComponent : BehaviourComponent {
        [SerializeField] AnimatedProgress expProgress;

        [SerializeField] TextMeshProUGUI exp;

        [SerializeField] TextMeshProUGUI currentRank;

        [SerializeField] TextMeshProUGUI nextRank;

        [SerializeField] TextMeshProUGUI winStats;

        [SerializeField] TextMeshProUGUI totalMatches;

        [SerializeField] LocalizedField expLocalizedField;

        [SerializeField] LocalizedField totalMatchesLocalizedField;

        [SerializeField] RankUI rank;

        [SerializeField] Color winColor;

        [SerializeField] Color lossColor;

        public GameObject showRewardsButton;

        public void SetLevelInfo(LevelInfo levelInfo, string rankName) {
            bool isMaxLevel = levelInfo.IsMaxLevel;
            nextRank.gameObject.SetActive(!isMaxLevel);
            expProgress.NormalizedValue = levelInfo.Progress;
            currentRank.text = (levelInfo.Level + 1).ToString();
            nextRank.text = (levelInfo.Level + 2).ToString();
            exp.text = !isMaxLevel ? string.Format(expLocalizedField.Value, levelInfo.Experience, levelInfo.MaxExperience) : levelInfo.Experience.ToString();
            rank.SetRank(levelInfo.Level, rankName);
        }

        public void SetWinLossStatistics(long winCount, long lossCount, long battlesCount) {
            winStats.text = "<color=#" + winColor.ToHexString() + ">" + winCount + "/<color=#" + lossColor.ToHexString() + ">" + lossCount;
            totalMatches.text = totalMatchesLocalizedField.Value + "\n" + battlesCount;
        }
    }
}