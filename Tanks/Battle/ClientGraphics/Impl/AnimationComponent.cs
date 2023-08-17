using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    [RequireComponent(typeof(Animator))]
    public class AnimationComponent : MonoBehaviour, Component {
        [SerializeField] Animator animator;

        public Animator Animator {
            get => animator;
            set => animator = value;
        }
    }
}