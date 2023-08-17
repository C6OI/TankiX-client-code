using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientControls.API {
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

        public int State { get; private set; }

        public int SpeedMultiplicatorId { get; private set; }

        public Animator Animator => animator;
    }
}