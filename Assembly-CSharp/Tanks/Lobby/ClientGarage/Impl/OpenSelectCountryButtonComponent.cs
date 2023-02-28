using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class OpenSelectCountryButtonComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI buttonTitle;

        [SerializeField] LocalizedField country;

        public string CountryCode {
            set => buttonTitle.text = country.Value + ": " + value;
        }
    }
}