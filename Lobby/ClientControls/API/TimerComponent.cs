using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientControls.API {
    public class TimerComponent : MonoBehaviour, Component {
        [SerializeField] TimerUIComponent timer;

        public TimerUIComponent Timer => timer;
    }
}