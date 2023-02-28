using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl.TankRent {
    public class TankRentMainScreenElementsComponents : BehaviourComponent {
        [SerializeField] GameObject tankRentScreen;

        public Button tankRentButton;

        public GameObject tankRentOffer;

        void Update() {
            if (InputMapping.Cancel && tankRentScreen.activeSelf) {
                HideTankRentScreen();
            }
        }

        void OnDisable() {
            tankRentButton.gameObject.SetActive(false);
        }

        public void SetButtonToOfferDisplayState() {
            tankRentButton.onClick.RemoveAllListeners();

            tankRentButton.onClick.AddListener(delegate {
                tankRentOffer.SetActive(true);
            });
        }

        public void SetButtonToScreenDisplayState() {
            tankRentButton.onClick.RemoveAllListeners();
            tankRentButton.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
            tankRentButton.onClick.AddListener(ShowTankRentScreen);
        }

        public void ShowTankRentScreen() {
            MainScreenComponent.Instance.OverrideOnBack(HideTankRentScreen);
            MainScreenComponent.Instance.OnPanelShow(MainScreenComponent.MainScreens.TankRent);
            tankRentScreen.SetActive(true);
        }

        public void HideTankRentScreen() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            MainScreenComponent.Instance.OnPanelShow(MainScreenComponent.MainScreens.Main);
            tankRentScreen.SetActive(false);
        }
    }
}