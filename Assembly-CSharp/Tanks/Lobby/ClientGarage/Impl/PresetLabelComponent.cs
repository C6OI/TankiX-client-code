using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PresetLabelComponent : BehaviourComponent {
        TextMeshProUGUI text => GetComponent<TextMeshProUGUI>();

        public string PresetName {
            get => text.text;
            set => text.text = value;
        }
    }
}