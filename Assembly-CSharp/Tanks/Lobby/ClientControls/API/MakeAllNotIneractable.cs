using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    public class MakeAllNotIneractable : MonoBehaviour {
        [ContextMenu("Make not interactable")]
        public void MakeNotInteractable() {
            Selectable[] componentsInChildren = GetComponentsInChildren<Selectable>(true);
            Selectable[] array = componentsInChildren;

            foreach (Selectable selectable in array) {
                selectable.interactable = false;
            }

            Image[] componentsInChildren2 = GetComponentsInChildren<Image>(true);
            Image[] array2 = componentsInChildren2;

            foreach (Image image in array2) {
                image.raycastTarget = false;
            }

            Text[] componentsInChildren3 = GetComponentsInChildren<Text>(true);
            Text[] array3 = componentsInChildren3;

            foreach (Text text in array3) {
                text.raycastTarget = false;
            }

            TextMeshProUGUI[] componentsInChildren4 = GetComponentsInChildren<TextMeshProUGUI>(true);
            TextMeshProUGUI[] array4 = componentsInChildren4;

            foreach (TextMeshProUGUI textMeshProUGUI in array4) {
                textMeshProUGUI.raycastTarget = false;
            }
        }
    }
}