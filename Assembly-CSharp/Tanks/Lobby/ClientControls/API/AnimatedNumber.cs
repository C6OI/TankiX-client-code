using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class AnimatedNumber : MonoBehaviour {
        [SerializeField] AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [SerializeField] TextMeshProUGUI numberText;

        [SerializeField] string format = "{0:#}";

        [SerializeField] float duration = 0.15f;

        float _currentValue;

        bool immediatePending;

        float targetValue = -1f;

        float time;

        float currentValue {
            get => _currentValue;
            set {
                _currentValue = value;
                numberText.text = string.Format(format, currentValue);
            }
        }

        public float Value {
            get => targetValue;
            set {
                targetValue = value;
                time = 0f;
            }
        }

        void Update() {
            if (currentValue != targetValue) {
                currentValue = Mathf.Lerp(currentValue, targetValue, curve.Evaluate(Mathf.Clamp01(time / duration)));
                time += Time.deltaTime;
            }
        }

        void OnEnable() {
            if (!immediatePending) {
                currentValue = 0f;
            }

            immediatePending = false;
        }

        public void SetFormat(string newFormat) {
            format = newFormat;
        }

        public void SetImmediate(float value) {
            targetValue = value;
            currentValue = targetValue;
            numberText.text = string.Format(format, value);
            immediatePending = !gameObject.activeInHierarchy;
        }
    }
}