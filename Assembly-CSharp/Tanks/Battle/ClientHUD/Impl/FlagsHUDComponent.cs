using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class FlagsHUDComponent : BehaviourComponent, AttachToEntityListener {
        [SerializeField] FlagController blueFlag;

        [SerializeField] RectTransform blueFlagTransform;

        [SerializeField] FlagController redFlag;

        [SerializeField] RectTransform redFlagTransform;

        int showRequests;

        public FlagController BlueFlag => blueFlag;

        public FlagController RedFlag => redFlag;

        public float RedFlagNormalizedPosition {
            set {
                if (value > 0.5f && blueFlagTransform.anchorMax.x < 0.5f) {
                    redFlagTransform.SetAsLastSibling();
                }

                SetFlagPosition(redFlagTransform, 1f - Mathf.Clamp01(value));
            }
        }

        public float BlueFlagNormalizedPosition {
            set {
                if (value > 0.5f && redFlagTransform.anchorMax.x > 0.5f) {
                    blueFlagTransform.SetAsLastSibling();
                }

                SetFlagPosition(blueFlagTransform, Mathf.Clamp01(value));
            }
        }

        void OnEnable() { }

        public void AttachedToEntity(Entity entity) {
            showRequests = 0;
        }

        public void RequestShow() { }

        public void RequestHide() { }

        void SetFlagPosition(RectTransform flag, float position) {
            Vector2 anchorMax = flag.anchorMin = new Vector2(position, 0f);
            flag.anchorMax = anchorMax;
            flag.anchoredPosition = new Vector2(0f, position != 0f && position != 1f ? -8.5f : 0f);
        }
    }
}