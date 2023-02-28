using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientControls.API {
    public class CancelButtonComponent : LocalizedControl, Component {
        [SerializeField] TextMeshProUGUI text;

        public virtual string Text {
            set => text.text = value;
        }
    }
}