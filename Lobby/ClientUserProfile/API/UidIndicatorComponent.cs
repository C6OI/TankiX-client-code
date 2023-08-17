using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientUserProfile.API {
    public class UidIndicatorComponent : UIBehaviour, Component {
        [SerializeField] Text uidText;

        public string Uid {
            get => uidText.text;
            set => uidText.text = value;
        }

        public Color Color {
            get => uidText.color;
            set => uidText.color = value;
        }
    }
}