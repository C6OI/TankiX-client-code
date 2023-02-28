using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleResultsScreenStatComponent : BehaviourComponent {
        [SerializeField] GameObject dmMatchDetails;

        [SerializeField] GameObject teamMatchDetails;

        [SerializeField] TextMeshProUGUI _battleDescription;

        public string BattleDescription {
            set => _battleDescription.text = value;
        }

        void OnDisable() {
            HideMatchDetails();
        }

        public void ShowDMMatchDetails() {
            dmMatchDetails.SetActive(true);
        }

        public void ShowTeamMatchDetails() {
            teamMatchDetails.SetActive(true);
        }

        public void HideMatchDetails() {
            dmMatchDetails.SetActive(false);
            teamMatchDetails.SetActive(false);
        }
    }
}