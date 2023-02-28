using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Battle.ClientControls.API {
    public class ProgressBarComponent : BehaviourComponent {
        [SerializeField] float progressValueOffset;

        ProgressBar progressBar;

        float targetProgresValue;

        public virtual float ProgressValue {
            get => targetProgresValue;
            set {
                targetProgresValue = ClampProgressValue(value, progressValueOffset);
                ProgressBar.ProgressValue = targetProgresValue;
            }
        }

        public ProgressBar ProgressBar {
            get {
                if (progressBar == null) {
                    progressBar = GetComponent<ProgressBar>();
                }

                return progressBar;
            }
        }

        protected virtual void Update() {
            if (ProgressBar.ProgressValue > targetProgresValue || targetProgresValue > 0.9f) {
                ProgressBar.ProgressValue = targetProgresValue;
            } else {
                ProgressBar.ProgressValue = Mathf.Clamp(ProgressBar.ProgressValue + Time.deltaTime * 0.05f, ProgressBar.ProgressValue, targetProgresValue);
            }
        }

        float ClampProgressValue(float realValue, float offset) {
            realValue = Mathf.Clamp01(realValue);

            if (realValue == 0f || realValue == 1f) {
                return realValue;
            }

            if (realValue < offset) {
                return offset;
            }

            if (realValue > 1f - offset) {
                return 1f - offset;
            }

            return realValue;
        }
    }
}