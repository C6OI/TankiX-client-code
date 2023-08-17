using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientControls.API;

namespace Tanks.Battle.ClientHUD.API {
    public class HealthBarComponent : ProgressBarComponent {
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

        void Update() {
            if (animating) {
                animatedValue += Time.deltaTime / animationTime;

                if (animatedValue >= 1f) {
                    animating = false;
                    base.ProgressValue = actualValue;
                }

                base.ProgressValue = animatedValue;
            }
        }

        public void ActivateAnimation(float timeInSec) {
            animationTime = timeInSec;
            animating = true;
            actualValue = base.ProgressValue;
            animatedValue = 0f;
        }
    }
}