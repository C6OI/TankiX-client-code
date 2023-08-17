using Lobby.ClientControls.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientLoading.API {
    public class ResourcesLoadProgressBarComponent : MonoBehaviour, Component {
        [SerializeField] float timeBeforeProgressCalculation = 0.1f;

        [SerializeField] float timeToFakeLoad = 2f;

        [SerializeField] float bytesToFakeLoad = 1048576f;

        public float TimeBeforeProgressCalculation {
            get => timeBeforeProgressCalculation;
            set => timeBeforeProgressCalculation = value;
        }

        public float TimeToFakeLoad {
            get => timeToFakeLoad;
            set => timeToFakeLoad = value;
        }

        public float BytesToFakeLoad {
            get => bytesToFakeLoad;
            set => bytesToFakeLoad = value;
        }

        public ProgressBar ProgressBar { get; set; }
    }
}