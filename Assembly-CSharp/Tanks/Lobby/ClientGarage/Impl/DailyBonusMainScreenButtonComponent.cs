using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DailyBonusMainScreenButtonComponent : BehaviourComponent {
        const string IS_ACTIVE = "isActive";

        [SerializeField] Animator animator;

        [SerializeField] CanvasGroup canvasGroup;

        [SerializeField] LocalizedField enabledTip;

        [SerializeField] LocalizedField disabledTipTip;

        [SerializeField] TooltipShowBehaviour tooltipShow;

        bool? interactable;

        bool? isActiveState;

        public bool IsActiveState {
            set {
                if (!isActiveState.HasValue) {
                    isActiveState = value;
                    animator.SetBool("isActive", value);
                } else if (isActiveState.Value ^ value) {
                    isActiveState = value;
                    animator.SetBool("isActive", value);
                }
            }
        }

        public bool Interactable {
            set {
                canvasGroup.interactable = value;
                canvasGroup.alpha = !value ? 0.5f : 1f;

                if (!interactable.HasValue) {
                    tooltipShow.localizedTip = !value ? disabledTipTip : enabledTip;
                    tooltipShow.UpdateTipText();
                    interactable = value;
                } else if (interactable.Value ^ value) {
                    tooltipShow.localizedTip = !value ? disabledTipTip : enabledTip;
                    tooltipShow.UpdateTipText();
                    interactable = value;
                }
            }
        }

        public void ResetState() {
            interactable = null;
            isActiveState = null;
        }
    }
}