using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientControls.API {
    public class UIListComponent : MonoBehaviour, Component {
        public IUIList List { get; private set; }

        void Awake() => List = GetComponent<IUIList>();
    }
}