using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AnimatorTimerComponent : MonoBehaviour, Component {
        public Animator animator;

        public string triggerName;

        public float timer;
    }
}