using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleSelectScreenLocalizationComponent : BehaviourComponent {
        [SerializeField] Text playButton;

        [SerializeField] Text archivedBattle;

        [SerializeField] Text archivedBattleTeam;

        [SerializeField] Text playRedButton;

        [SerializeField] Text playBlueButton;

        [SerializeField] Text watchButton;

        [SerializeField] Text redTeamName;

        [SerializeField] Text blueTeamName;

        public string PlayButton {
            set => playButton.text = value;
        }

        public string PlayRedButton {
            set => playRedButton.text = value;
        }

        public string PlayBlueButton {
            set => playBlueButton.text = value;
        }

        public string WatchButton {
            set => watchButton.text = value;
        }

        public string BattleLevelsIndicatorText { get; set; }

        public string LevelWarningEquipDowngradedText { get; set; }

        public string LevelWarningXpReducedText { get; set; }

        public string LevelWarningEquipDowngradedAndXpReducedText { get; set; }

        public string LevelErrorText { get; set; }

        public string ArchivedBattleText {
            set {
                archivedBattle.text = value;
                archivedBattleTeam.text = value;
            }
        }

        public string RedTeamName {
            set => redTeamName.text = value;
        }

        public string BlueTeamName {
            set => blueTeamName.text = value;
        }
    }
}