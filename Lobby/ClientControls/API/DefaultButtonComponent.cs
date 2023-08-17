using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientControls.API {
    public class DefaultButtonComponent : LocalizedControl, Component {
        [SerializeField] Text text;

        public virtual string Text {
            set => text.text = value.ToUpper();
        }
    }
}