using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class ScoreTablePingIndicatorComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI pingCount;

        public void SetPing(int ping) {
            pingCount.text = ping.ToString();
        }
    }
}