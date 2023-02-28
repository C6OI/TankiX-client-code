using UnityEngine;
using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientControls.API {
    public class InputFieldDoubleClickSelector : MonoBehaviour {
        static readonly float DOUBLE_CLICK_DELTA_MIN = 0.05f;

        static readonly float DOUBLE_CLICK_DELTA_MAX = 0.3f;

        public InputFieldComponent inputField;

        bool firstClick;

        bool selected;

        float selectionTime;

        float timeBetweenClicks;

        void Update() {
            selected = EventSystem.current.currentSelectedGameObject == inputField.InputFieldGameObject;

            if (selected) {
                selectionTime += Time.deltaTime;
            } else {
                selectionTime = 0f;
            }

            if (firstClick) {
                timeBetweenClicks += Time.deltaTime;
            }

            if (timeBetweenClicks >= DOUBLE_CLICK_DELTA_MAX) {
                timeBetweenClicks = 0f;
                firstClick = false;
            }
        }

        public void OnMouseClick() {
            if (!(selectionTime < DOUBLE_CLICK_DELTA_MAX)) {
                firstClick = !firstClick;

                if (timeBetweenClicks >= DOUBLE_CLICK_DELTA_MIN && timeBetweenClicks <= DOUBLE_CLICK_DELTA_MAX) {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(inputField.InputFieldGameObject);
                    firstClick = false;
                    timeBetweenClicks = 0f;
                }
            }
        }
    }
}