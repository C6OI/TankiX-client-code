using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleEffectsInfoComponent : UIBehaviour, Component {
        [SerializeField] TextMeshProUGUI effectText;

        [SerializeField] ImageSkin effectIcon;

        [SerializeField] PaletteColorField exceptionalColor;

        [SerializeField] PaletteColorField epicColor;

        [SerializeField] Image staticIcon;

        public ImageSkin EffectIcon => effectIcon;

        public string EffectValue {
            set => effectText.text = value;
        }

        public Color ExceptionalColor => exceptionalColor;

        public Color EpicColor => epicColor;

        public Image StaticIcon => staticIcon;

        public TextMeshProUGUI EffectText => effectText;
    }
}