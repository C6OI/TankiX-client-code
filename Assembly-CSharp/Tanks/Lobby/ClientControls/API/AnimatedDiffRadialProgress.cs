using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    public class AnimatedDiffRadialProgress : MonoBehaviour {
        [SerializeField] AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [SerializeField] Image fill;

        [SerializeField] Image background;

        [SerializeField] Image diff;

        [SerializeField] float duration = 0.15f;

        [SerializeField] float normalizedValue;

        [SerializeField] float newValue;

        Coroutine coroutine;

        void Awake() {
            fill.fillAmount = 0f;
            diff.fillAmount = 0f;
        }

        void Update() {
            if (!Application.isPlaying) {
                diff.fillAmount = normalizedValue;
                fill.fillAmount = normalizedValue + (newValue - normalizedValue);
            }
        }

        void OnEnable() {
            StartCoroutine(AnimateTo(0f, normalizedValue, normalizedValue, newValue));
        }

        public void Set(float value, float newValue) {
            if (coroutine != null) {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            if (gameObject.activeInHierarchy) {
                coroutine = StartCoroutine(AnimateTo(normalizedValue, value, value, newValue));
            }

            normalizedValue = value;
            this.newValue = newValue;
        }

        IEnumerator AnimateTo(float startValue, float targetValue, float startNewValue, float targetNewValue) {
            float time = 0f;
            float value2 = startValue;

            for (; time < duration; time += Time.deltaTime) {
                value2 = startValue + (targetValue - startValue) * curve.Evaluate(Mathf.Clamp01(time / duration));
                diff.fillAmount = value2;
                fill.fillAmount = startNewValue + (targetNewValue - startNewValue) * curve.Evaluate(Mathf.Clamp01(time / duration));
                yield return null;
            }
        }
    }
}