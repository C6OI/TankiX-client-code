using Lobby.ClientControls.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientControls.API {
    public class ProgressBarComponent : MonoBehaviour, Component {
        [SerializeField] float progressValueOffset;

        ProgressBar progressBar;

        public virtual float ProgressValue {
            get => ProgressBar.ProgressValue;
            set => ProgressBar.ProgressValue = ClampProgressValue(value, progressValueOffset);
        }

        ProgressBar ProgressBar {
            get {
                if (progressBar == null) {
                    progressBar = GetComponent<ProgressBar>();
                }

                return progressBar;
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