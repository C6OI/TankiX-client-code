using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ContainerDescriptionUI : MonoBehaviour {
        [SerializeField] TextMeshProUGUI title;

        [SerializeField] TextMeshProUGUI description;

        public TextMeshProUGUI Title {
            get => title;
            set => title = value;
        }

        public TextMeshProUGUI Description {
            get => description;
            set => description = value;
        }
    }
}