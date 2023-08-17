using Lobby.ClientNavigation.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class
        ChangeCountryButtonComponent : ButtonMappingComponentBase<ShowScreenLeftEvent<SelectCountryScreenComponent>> {
        [SerializeField] Text countryCode;

        public string CountryCode {
            set => countryCode.text = value;
        }
    }
}