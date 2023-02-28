using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PremiumMainScreenButtonComponent : BehaviourComponent {
        public GameObject activePremiumIcon;

        public GameObject inactivePremiumIcon;

        public void ActivatePremium() {
            ActivatePremium(true);
        }

        public void DeactivatePremium() {
            ActivatePremium(false);
        }

        void ActivatePremium(bool val) {
            activePremiumIcon.SetActive(val);
            inactivePremiumIcon.SetActive(!val);
        }
    }
}