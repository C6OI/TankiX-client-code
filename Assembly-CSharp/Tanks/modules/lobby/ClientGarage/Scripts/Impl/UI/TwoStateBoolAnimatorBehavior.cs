using UnityEngine;
using UnityEngine.EventSystems;

namespace tanks.modules.lobby.ClientGarage.Scripts.Impl.UI {
    public class TwoStateBoolAnimatorBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        [SerializeField] Animator _targetAnimator;

        [SerializeField] string _boolTriggerName;

        public void OnPointerEnter(PointerEventData eventData) {
            if (!(_targetAnimator == null) && !string.IsNullOrEmpty(_boolTriggerName)) {
                _targetAnimator.SetBool(_boolTriggerName, true);
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            if (!(_targetAnimator == null) && !string.IsNullOrEmpty(_boolTriggerName)) {
                _targetAnimator.SetBool(_boolTriggerName, false);
            }
        }
    }
}