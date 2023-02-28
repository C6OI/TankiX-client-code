using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableHullIndicatorComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI hullIcon;

        public void SetHullIcon(long id) {
            hullIcon.text = "<sprite name=\"" + id + "\">";
        }
    }
}