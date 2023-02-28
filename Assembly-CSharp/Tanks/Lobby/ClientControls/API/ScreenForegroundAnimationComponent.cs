using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientControls.API {
    public class ScreenForegroundAnimationComponent : MonoBehaviour, Component {
        [SerializeField] Animator animator;

        public Animator Animator => animator;

        public float Alpha {
            get => GetComponent<CanvasGroup>().alpha;
            set => GetComponent<CanvasGroup>().alpha = value;
        }
    }
}