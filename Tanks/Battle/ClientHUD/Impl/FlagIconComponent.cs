using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class FlagIconComponent : MonoBehaviour, Component {
        [SerializeField] Animator animator;

        [SerializeField] string flashStateName;

        [SerializeField] string explosionTrigerName;

        [SerializeField] string speedFactorName;

        [SerializeField] float minSpeedFlash;

        [SerializeField] float maxSpeedFlash;

        public Animator Animator => animator;

        public string FlashStateName => flashStateName;

        public string ExplosionTrigerName => explosionTrigerName;

        public string SpeedFactorName => speedFactorName;

        public float MinSpeedFlash => minSpeedFlash;

        public float MaxSpeedFlash => maxSpeedFlash;
    }
}