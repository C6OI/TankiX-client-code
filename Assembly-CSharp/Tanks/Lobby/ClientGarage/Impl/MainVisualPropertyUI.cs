using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class MainVisualPropertyUI : MonoBehaviour {
        [SerializeField] TextMeshProUGUI text;

        [SerializeField] AnimatedProgress progress;

        public void Set(string name, float progress) {
            this.progress.NormalizedValue = progress;
            text.text = name;
        }
    }
}