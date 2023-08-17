using Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class TeamBattleScoreIndicatorComponent : MonoBehaviour, Component {
        [SerializeField] Text blueTeamScoreText;

        [SerializeField] Text redTeamScoreText;

        [SerializeField] ProgressBar blueScoreProgress;

        [SerializeField] ProgressBar redScoreProgress;

        public void UpdateScore(int blueScore, int redScore, int scoreLimit) {
            blueTeamScoreText.text = blueScore.ToString();
            redTeamScoreText.text = redScore.ToString();
            blueScoreProgress.ProgressValue = scoreLimit <= 0 ? 0f : blueScore / (float)scoreLimit;
            redScoreProgress.ProgressValue = scoreLimit <= 0 ? 0f : redScore / (float)scoreLimit;
        }
    }
}