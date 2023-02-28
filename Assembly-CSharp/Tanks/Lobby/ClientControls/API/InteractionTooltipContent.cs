using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class InteractionTooltipContent : BehaviourComponent, ITooltipContent {
        [SerializeField] GameObject responceMessagePrefab;

        RectTransform rect;

        protected virtual void Awake() {
            rect = GetComponent<RectTransform>();
        }

        public void Update() {
            bool flag = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
            bool keyUp = Input.GetKeyUp(KeyCode.Escape);

            if (flag && PointerOutsideMenu() || keyUp) {
                Hide();
            }
        }

        public virtual void Init(object data) { }

        public void Hide() {
            TooltipController.Instance.HideTooltip();
        }

        bool PointerOutsideMenu() => !RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition);

        protected void ShowResponse(string respondText) {
            GameObject gameObject = Instantiate(responceMessagePrefab);
            gameObject.transform.SetParent(transform.parent.parent, false);
            gameObject.GetComponent<RectTransform>().position = Input.mousePosition;
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = respondText;
            gameObject.SetActive(true);
            float length = gameObject.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, length);
        }
    }
}