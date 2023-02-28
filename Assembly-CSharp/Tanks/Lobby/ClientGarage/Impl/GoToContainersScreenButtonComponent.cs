using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GoToContainersScreenButtonComponent : LocalizedControl, Component {
        [SerializeField] Text text;

        public virtual string Text {
            set => text.text = value.ToUpper();
        }
    }
}