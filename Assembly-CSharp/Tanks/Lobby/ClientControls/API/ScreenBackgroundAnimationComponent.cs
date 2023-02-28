using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientControls.API {
    public class ScreenBackgroundAnimationComponent : MonoBehaviour, Component {
        [SerializeField] int layerId;

        [SerializeField] string state = "Show";

        [SerializeField] string speedMultiplicatorName = "showSpeed";

        [SerializeField] Animator animator;

        public ScreenBackgroundAnimationComponent() {
            State = Animator.StringToHash(state);
            SpeedMultiplicatorId = Animator.StringToHash(speedMultiplicatorName);
        }

        public int LayerId => layerId;

        public int State { get; }

        public int SpeedMultiplicatorId { get; }

        public Animator Animator => animator;
    }
}