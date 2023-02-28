using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientNavigation.API {
    public class LinkButtonComponent : UIBehaviour, Component {
        [SerializeField] string link;

        public string Link {
            get => link;
            set => link = value;
        }
    }
}