using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [RequireComponent(typeof(LayoutElement))]
    [RequireComponent(typeof(CanvasGroup))]
    public class ShowIndicatorOnRoundRestartComponent : MonoBehaviour, Component {
        public void Show() {
            GetComponent<CanvasGroup>().alpha = 1f;
            GetComponent<LayoutElement>().ignoreLayout = false;
        }

        public void Hide() {
            GetComponent<CanvasGroup>().alpha = 0f;
            GetComponent<LayoutElement>().ignoreLayout = true;
        }
    }
}