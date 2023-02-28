using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class LeaguePlaceUIComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI placeText;

        [SerializeField] LocalizedField placeLocalizedField;

        [SerializeField] List<GameObject> elements;

        public void SetPlace(int place) {
            placeText.text = placeLocalizedField.Value + "\n" + place;
            Show();
        }

        public void Hide() {
            SetElementsVisibility(false);
        }

        void Show() {
            SetElementsVisibility(true);
        }

        void SetElementsVisibility(bool visibility) {
            elements.ForEach(delegate(GameObject element) {
                element.SetActive(visibility);
            });
        }
    }
}