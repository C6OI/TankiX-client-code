using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class TooltipController : MonoBehaviour {
        public Tooltip tooltip;

        public float delayBeforeTooltipShowAfterCursorStop = 0.1f;

        public float maxDelayForQuickShowAfterCursorStop = 0.2f;

        [HideInInspector] public bool quickShow;

        public float delayBeforeQuickShow = 0.1f;

        float afterHideTimer;

        bool tooltipIsShow;

        public static TooltipController Instance { get; private set; }

        void Awake() {
            Instance = this;
        }

        void Update() {
            if (!tooltipIsShow && quickShow) {
                afterHideTimer += Time.deltaTime;

                if (afterHideTimer > maxDelayForQuickShowAfterCursorStop) {
                    quickShow = false;
                }
            }
        }

        public void ShowTooltip(Vector3 position, object data, GameObject tooltipContentPrefab = null, bool backgroundActive = true) {
            tooltipIsShow = true;
            Vector2 localPoint;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), position, null, out localPoint)) {
                tooltip.Show(localPoint, data, tooltipContentPrefab, backgroundActive);
            }
        }

        public void HideTooltip() {
            afterHideTimer = 0f;
            quickShow = true;
            tooltipIsShow = false;
            tooltip.Hide();
        }
    }
}