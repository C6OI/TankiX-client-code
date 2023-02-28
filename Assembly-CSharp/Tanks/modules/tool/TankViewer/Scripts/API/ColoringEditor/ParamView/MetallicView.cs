using UnityEngine;
using UnityEngine.UI;

namespace tanks.modules.tool.TankViewer.Scripts.API.ColoringEditor.ParamView {
    public class MetallicView : MonoBehaviour {
        public Slider slider;

        public void SetFloat(float value) {
            slider.value = value;
        }

        public float GetFloat() => slider.value;
    }
}