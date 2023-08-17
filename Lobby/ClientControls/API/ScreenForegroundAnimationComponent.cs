using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientControls.API {
    public class ScreenForegroundAnimationComponent : MonoBehaviour, Component {
        [SerializeField] Animator animator;

        public Animator Animator => animator;
    }
}