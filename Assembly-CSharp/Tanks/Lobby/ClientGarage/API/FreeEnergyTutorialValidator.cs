using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.API {
    public class FreeEnergyTutorialValidator : MonoBehaviour, ITutorialShowStepValidator {
        public bool ShowAllowed(long stepId) {
            if (MainScreenComponent.Instance != null) {
                return MainScreenComponent.Instance.GetCurrentPanel() != MainScreenComponent.MainScreens.Shop;
            }

            return false;
        }
    }
}