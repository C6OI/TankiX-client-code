using System.Collections;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    public class KillAssistEventLogMessage : CombatEventLogMessage {
        public override void Attach(RectTransform child, bool toRight) {
            child.SetParent(placeholder, false);

            if (toRight) {
                if (rightElement != null) {
                    rightElement.SetParent(child, false);
                    LayoutElement layoutElement = rightElement.GetComponent<LayoutElement>() ?? rightElement.gameObject.AddComponent<LayoutElement>();
                    layoutElement.ignoreLayout = true;
                    StartCoroutine(DisplaceParent(rightElement));
                }

                rightElement = child;
            }
        }

        IEnumerator DisplaceParent(RectTransform nick) {
            yield return new WaitForEndOfFrame();

            Vector2 position = rightElement.anchoredPosition;
            position.x += nick.rect.width;
            rightElement.anchoredPosition = position;
        }
    }
}