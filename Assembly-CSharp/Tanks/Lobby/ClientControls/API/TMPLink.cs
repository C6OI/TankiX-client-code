using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class TMPLink : MonoBehaviour {
        Camera cam;

        Canvas canvas;

        int selectedLink = -1;

        TMP_Text tmpText;

        void Start() {
            tmpText = gameObject.GetComponent<TMP_Text>();

            if (tmpText.GetType() == typeof(TextMeshProUGUI)) {
                canvas = gameObject.GetComponentInParent<Canvas>();

                if (canvas != null) {
                    if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
                        cam = null;
                    } else {
                        cam = canvas.worldCamera;
                    }
                }
            } else {
                cam = Camera.main;
            }
        }

        void LateUpdate() {
            if (TMP_TextUtilities.IsIntersectingRectTransform(tmpText.rectTransform, Input.mousePosition, cam)) {
                int num = TMP_TextUtilities.FindIntersectingLink(tmpText, Input.mousePosition, cam);

                if (selectedLink != -1 && num != selectedLink) {
                    UnhighlightLink(selectedLink);
                }

                if (num != -1 && num != selectedLink) {
                    HighlightLink(num);
                }

                selectedLink = num;
            }

            if (Input.GetMouseButtonDown(0) && selectedLink != -1) {
                Application.OpenURL(tmpText.textInfo.linkInfo[selectedLink].GetLinkID());
            }
        }

        void HighlightLink(int linkIndex) {
            TMP_LinkInfo tMP_LinkInfo = tmpText.textInfo.linkInfo[linkIndex];
            string text = "<u><link=" + tMP_LinkInfo.GetLinkID() + ">" + tMP_LinkInfo.GetLinkText() + "</link></u>";
            tmpText.text = tmpText.text.Replace(text, GetColoredLinkText(text));
            Cursors.SwitchToCursor(CursorType.HAND);
        }

        void UnhighlightLink(int linkIndex) {
            TMP_LinkInfo tMP_LinkInfo = tmpText.textInfo.linkInfo[linkIndex];
            string text = "<u><link=" + tMP_LinkInfo.GetLinkID() + ">" + tMP_LinkInfo.GetLinkText() + "</link></u>";
            tmpText.text = tmpText.text.Replace(GetColoredLinkText(text), text);
            Cursors.SwitchToDefaultCursor();
        }

        string GetColoredLinkText(string text) => "<color=blue>" + text + "</color>";
    }
}