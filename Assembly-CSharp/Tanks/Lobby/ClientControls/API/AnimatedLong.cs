using System.Collections;
using Platform.Library.ClientLocale.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class AnimatedLong : MonoBehaviour {
        [SerializeField] AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [SerializeField] float duration = 0.15f;

        [SerializeField] TextMeshProUGUI numberText;

        [SerializeField] string format = "{0:#}";

        Animator animator;

        Coroutine coroutine;

        bool immediatePending;

        long value = -1L;

        public long Value {
            get => value;
            set {
                if (this.value != value) {
                    if (coroutine != null) {
                        StopCoroutine(coroutine);
                        coroutine = null;
                    }

                    StopAnimation();

                    if (gameObject.activeInHierarchy) {
                        coroutine = StartCoroutine(AnimateTo(this.value, value));
                    }

                    this.value = value;
                }
            }
        }

        void OnEnable() {
            animator = GetComponent<Animator>();

            if (!immediatePending) {
                coroutine = StartCoroutine(AnimateTo(0L, value));
            }

            immediatePending = false;
        }

        public void SetFormat(string newFormat) {
            format = newFormat;
        }

        public void SetImmediate(long value) {
            if (coroutine != null) {
                StopCoroutine(coroutine);
                coroutine = null;
            }

            StopAnimation();
            this.value = value;
            SetText(value);
            immediatePending = !gameObject.activeInHierarchy;
        }

        IEnumerator AnimateTo(long startValue, long targetValue) {
            if (targetValue > startValue) {
                StartAnimation();
            }

            float time = 0f;
            long val = startValue;

            while (!Mathf.Approximately(val, targetValue)) {
                val = startValue + (long)((targetValue - startValue) * curve.Evaluate(Mathf.Clamp01(time / duration)));
                SetText(val);
                yield return null;

                time += Time.deltaTime;
            }

            StopAnimation();
        }

        void SetText(long val) {
            numberText.text = string.Format(LocaleUtils.GetCulture(), format, val);
        }

        void StartAnimation() {
            if (animator != null) {
                animator.SetBool("animated", true);
            }
        }

        void StopAnimation() {
            if (animator != null) {
                animator.SetBool("animated", false);
            }
        }
    }
}