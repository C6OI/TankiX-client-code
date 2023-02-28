using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTableTurretIndicatorComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI turretIcon;

        public void SetTurretIcon(long id) {
            turretIcon.text = "<sprite name=\"" + id + "\">";
        }
    }
}