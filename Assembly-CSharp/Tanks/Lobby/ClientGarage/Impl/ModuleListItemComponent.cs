using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleListItemComponent : UIBehaviour, Component {
        [SerializeField] GameObject moduleEffectsInfoPrefab;

        [SerializeField] RectTransform moduleIconRoot;

        [SerializeField] RectTransform moduleNameRoot;

        [SerializeField] TextMeshProUGUI craftableLabelText;

        [SerializeField] RectTransform moduleEffectsInfoRoot;

        [SerializeField] TextMeshProUGUI moduleNameText;

        [SerializeField] PaletteColorField exceptionalColor;

        [SerializeField] PaletteColorField epicColor;

        [SerializeField] Image moduleIcon;

        [SerializeField] TextMeshProUGUI moduleText;

        [SerializeField] Color craftableTextColor;

        [SerializeField] Color notCraftableTextColor;

        [SerializeField] GameObject mountedSelection;

        public GameObject ModuleEffectsInfoPrefab => moduleEffectsInfoPrefab;

        public RectTransform ModuleEffectsInfoRoot => moduleEffectsInfoRoot;

        public string IconUid {
            set {
                moduleIconRoot.gameObject.SetActive(true);
                moduleIconRoot.GetComponent<ImageSkin>().SpriteUid = value;
            }
        }

        public string Name {
            set {
                moduleNameRoot.gameObject.SetActive(true);
                moduleNameText.text = value;
            }
        }

        public string CraftableText {
            set {
                craftableLabelText.gameObject.SetActive(true);
                craftableLabelText.text = value;
            }
        }

        public Color TextColor {
            set => craftableLabelText.color = value;
        }

        public Color CraftableTextColor => craftableTextColor;

        public Color NotCraftableTextColor => notCraftableTextColor;

        public Color ExceptionalColor => exceptionalColor;

        public Color EpicColor => epicColor;

        public Image ModuleIcon => moduleIcon;

        public Graphic ModuleText => moduleText;

        public bool MountedSelectionActivity {
            get => mountedSelection.activeSelf;
            set => mountedSelection.SetActive(value);
        }
    }
}