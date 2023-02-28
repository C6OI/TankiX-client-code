using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModulesNotificationBadgeComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI text;

        [SerializeField] LocalizedField newModuleAvailable;

        [SerializeField] LocalizedField moduleUpgradeAvailable;

        public TankPartModuleType TankPart;

        State currentState;

        public State CurrentState {
            get => currentState;
            set {
                text.gameObject.SetActive(false);
                text.text = "<color=#F2A00CFF>";
                currentState = value;

                switch (currentState) {
                    case State.NewModuleAvailable:
                        text.text += newModuleAvailable.Value;
                        text.gameObject.SetActive(true);
                        break;

                    case State.ModuleUpgradeAvailable:
                        text.text += moduleUpgradeAvailable.Value;
                        text.gameObject.SetActive(true);
                        break;
                }
            }
        }
    }
}