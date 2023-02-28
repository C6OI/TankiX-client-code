using UnityEngine;
using UnityEngine.UI;

namespace tanks.modules.tool.TankViewer.Scripts.API.ColoringEditor.ParamView {
    public class SmoothnessView : MonoBehaviour {
        public Toggle toggle;

        public Slider slider;

        public void SetOverrideSmoothness(bool value) {
            toggle.isOn = value;

            if (toggle.interactable) {
                Enable();
            }
        }

        public bool GetOverrideSmoothness() => toggle.isOn;

        public void SetSmoothnessStrenght(float value) {
            slider.value = value;
        }

        public float GetSmoothnessStrenght() => slider.value;

        public void OnToggleChanged() {
            Enable();
        }

        public void Enable() {
            toggle.interactable = true;
            slider.interactable = toggle.isOn;
        }

        public void Disable() {
            toggle.interactable = false;
            slider.interactable = false;
        }
    }
}