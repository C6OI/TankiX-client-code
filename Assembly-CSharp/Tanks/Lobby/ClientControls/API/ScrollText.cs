using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    public class ScrollText : MonoBehaviour {
        [SerializeField] Text text;

        public virtual string Text {
            get => text.text;
            set => text.text = value;
        }
    }
}