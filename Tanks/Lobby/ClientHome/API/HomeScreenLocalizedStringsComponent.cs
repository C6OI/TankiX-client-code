using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientHome.API {
    public class HomeScreenLocalizedStringsComponent : BehaviourComponent {
        [SerializeField] Text battlesButtonLabel;

        [SerializeField] Text garageButtonLabel;

        public virtual string BattlesButtonLabel {
            set => battlesButtonLabel.text = value;
        }

        public virtual string GarageButtonLabel {
            set => garageButtonLabel.text = value;
        }
    }
}