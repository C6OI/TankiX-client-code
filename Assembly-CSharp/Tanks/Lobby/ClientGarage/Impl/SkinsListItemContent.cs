using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SkinsListItemContent : MonoBehaviour, ListItemContent {
        [SerializeField] TextMeshProUGUI nameLabel;

        public void SetDataProvider(object dataProvider) {
            VisualItem visualItem = (VisualItem)dataProvider;
            nameLabel.text = visualItem.Name;
        }

        public void Select() { }
    }
}