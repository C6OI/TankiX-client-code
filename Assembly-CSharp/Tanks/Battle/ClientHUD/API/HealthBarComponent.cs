using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientControls.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.API {
    public class HealthBarComponent : ProgressBarComponent {
        CanvasGroup _canvasGroup;

        float _defaultAlpha;
        float actualValue;

        float animatedValue;

        bool animating;

        float animationTime;

        [Inject] public static UnityTime Time { get; set; }

        public override float ProgressValue {
            get {
                if (animating) {
                    return actualValue;
                }

                return base.ProgressValue;
            }
            set {
                if (animating) {
                    actualValue = value;
                } else {
                    base.ProgressValue = value;
                }
            }
        }

        protected override void Update() {
            base.Update();

            if (animating) {
                animatedValue += Time.deltaTime / animationTime;

                if (animatedValue >= 1f) {
                    animating = false;
                    base.ProgressValue = actualValue;
                }

                base.ProgressValue = animatedValue;
            }
        }

        void OnEnable() {
            _canvasGroup = GetComponent<CanvasGroup>();
            _defaultAlpha = _canvasGroup.alpha;
        }

        public void HideHealthBar() {
            _canvasGroup.alpha = 0f;
            gameObject.transform.SetAsFirstSibling();
        }

        public void ShowHealthBar() {
            _canvasGroup.alpha = _defaultAlpha;
            gameObject.transform.SetAsLastSibling();
        }

        public void ActivateAnimation(float timeInSec) {
            animationTime = timeInSec;
            animating = true;
            actualValue = base.ProgressValue;
            animatedValue = 0f;
        }
    }
}