using Platform.Kernel.ECS.ClientEntitySystem.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PresetListItemComponent : UIBehaviour, Component {
        [SerializeField] GameObject iconObject;

        [SerializeField] TextMeshProUGUI text;

        [SerializeField] Graphic bgGraphic;

        [SerializeField] Color lockedColor;

        [SerializeField] Color unlockedColor;

        bool locked;

        public Entity Preset { get; set; }

        public bool IsUserItem { get; set; }

        public bool IsOwned { get; set; }

        public string PresetName {
            get => text.text;
            set => text.text = value;
        }

        public int Rank { get; set; }

        public bool Locked {
            get => locked;
            set {
                locked = value;
                iconObject.SetActive(value);
                bgGraphic.color = !value ? unlockedColor : lockedColor;
            }
        }
    }
}