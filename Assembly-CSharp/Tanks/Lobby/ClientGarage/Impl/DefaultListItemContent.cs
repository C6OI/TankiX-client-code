using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DefaultListItemContent : MonoBehaviour, ListItemContent {
        [SerializeField] TextMeshProUGUI nameLabel;

        public void SetDataProvider(object dataProvider) {
            nameLabel.text = (string)dataProvider;
        }

        public void Select() { }
    }
}