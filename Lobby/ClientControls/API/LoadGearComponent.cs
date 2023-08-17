using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientControls.API {
    public class LoadGearComponent : MonoBehaviour, Component {
        [SerializeField] Animator animator;

        [SerializeField] ProgressBar gearProgressBar;

        public Animator Animator => animator;

        public ProgressBar GearProgressBar => gearProgressBar;

        void Hide() => gameObject.SetActive(false);
    }
}