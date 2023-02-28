using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableFlagIndicatorComponent : BehaviourComponent {
        [SerializeField] GameObject flagIcon;

        public void SetFlagIconActivity(bool activity) {
            flagIcon.SetActive(activity);
        }
    }
}