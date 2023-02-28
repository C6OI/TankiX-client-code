using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SelectCountryItem : MonoBehaviour {
        [SerializeField] TextMeshProUGUI countryName;

        KeyValuePair<string, string> country;

        public CountrySelected countrySelected;

        public string CountryName {
            get => country.Value;
            set => countryName.text = value;
        }

        public string CountryCode => country.Key;

        void Awake() {
            GetComponent<Toggle>().onValueChanged.AddListener(ToggleValueChanged);
        }

        void OnDestroy() {
            countrySelected = null;
        }

        public void Init(KeyValuePair<string, string> country) {
            this.country = country;
            CountryName = country.Value;
        }

        void ToggleValueChanged(bool value) {
            if (value && countrySelected != null) {
                countrySelected(country);
            }
        }
    }
}