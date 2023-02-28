using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    public class MakeKeyboardSettingsButton : MonoBehaviour {
        [ContextMenu("Make")]
        public void Make() {
            Debug.Log("Make for " + this.gameObject.name);
            MakeAllNotIneractable makeAllNotIneractable = this.gameObject.AddComponent<MakeAllNotIneractable>();
            makeAllNotIneractable.MakeNotInteractable();
            DestroyImmediate(makeAllNotIneractable);
            Debug.Log("Not interactable");
            InputField[] componentsInChildren = GetComponentsInChildren<InputField>();
            InputField[] array = componentsInChildren;

            foreach (InputField inputField in array) {
                Debug.Log(inputField.name);
                GameObject gameObject = new("Button");
                gameObject.transform.SetParent(transform, false);
                gameObject.AddComponent<Button>();
                gameObject.AddComponent<CursorSwitcher>();
                gameObject.AddComponent<InputFieldParentButton>();
                Image image = gameObject.AddComponent<Image>();
                image.color = Color.clear;
                RectTransform component = inputField.GetComponent<RectTransform>();
                RectTransform component2 = gameObject.GetComponent<RectTransform>();
                component2.pivot = component.pivot;
                component2.anchorMax = component.anchorMax;
                component2.anchorMin = component.anchorMin;
                component2.anchoredPosition = component.anchoredPosition;
                component2.offsetMin = component.offsetMin;
                component2.offsetMax = component.offsetMax;
                inputField.transform.SetParent(gameObject.transform, false);
                component.anchorMin = Vector2.zero;
                component.anchorMax = Vector2.one;
                Vector2 offsetMin = component.offsetMax = Vector2.zero;
                component.offsetMin = offsetMin;
            }

            Debug.Log("Done");
        }
    }
}