using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class DMBattleResultsScreenComponent : LocalizedScreenComponent, NoScaleScreen {
        [SerializeField] Text selfScore;

        [SerializeField] Text maxScore;

        [SerializeField] ProgressBar progressBar;

        [SerializeField] Text mapName;

        public void Init(int selfScore, int maxScore, string mapName) {
            if (!(progressBar == null)) {
                progressBar.ProgressValue = maxScore != 0 ? Mathf.Clamp01(selfScore / (float)maxScore) : 1f;
                this.selfScore.text = selfScore.ToString();
                this.maxScore.text = maxScore.ToString();
                this.mapName.text = mapName.ToUpper();
            }
        }
    }
}