using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class UserSquadColorComponent : BehaviourComponent {
        [SerializeField] Image image;

        public Image Image {
            set => image = value;
        }

        public Color Color {
            set => image.color = value;
        }
    }
}