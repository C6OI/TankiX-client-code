using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class TeamBattleResultsScreenComponent : LocalizedScreenComponent, NoScaleScreen {
        [SerializeField] TextMeshProUGUI blueScore;

        [SerializeField] TextMeshProUGUI redScore;

        [SerializeField] TextMeshProUGUI blueTeamTitle;

        [SerializeField] TextMeshProUGUI redTeamTitle;

        [SerializeField] TextMeshProUGUI blueTeamTitleForSpectator;

        [SerializeField] TextMeshProUGUI redTeamTitleForSpectator;

        public void Init(string mode, int blueScore, int redScore, string mapName, bool spectator) {
            this.blueScore.text = blueScore.ToString();
            this.redScore.text = redScore.ToString();
            blueTeamTitleForSpectator.gameObject.SetActive(spectator);
            redTeamTitleForSpectator.gameObject.SetActive(spectator);
            blueTeamTitle.gameObject.SetActive(!spectator);
            redTeamTitle.gameObject.SetActive(!spectator);
        }
    }
}