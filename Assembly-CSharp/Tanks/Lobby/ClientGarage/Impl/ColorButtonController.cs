using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    [ExecuteInEditMode]
    public class ColorButtonController : MonoBehaviour {
        [SerializeField] public List<ColorData> colors = new();

        ColorData currentColor = new();

        readonly List<IColorButtonElement> elements = new();

        ColorData lastColor;

        public void Awake() {
            currentColor = GetDefaultColor();
        }

        public void AddElement(IColorButtonElement element) {
            element.SetColor(currentColor);
            elements.Add(element);
        }

        public void RemoveElement(IColorButtonElement element) {
            elements.Remove(element);
        }

        void SetColor(ColorData color) {
            for (int i = 0; i < elements.Count; i++) {
                elements[i].SetColor(color);
            }
        }

        public void SetDefault() {
            ColorData defaultColor = GetDefaultColor();

            for (int i = 0; i < elements.Count; i++) {
                elements[i].SetColor(defaultColor);
            }

            currentColor = defaultColor;
        }

        ColorData GetDefaultColor() {
            ColorData result = new();

            for (int i = 0; i < colors.Count; i++) {
                ColorData colorData = colors[i];

                if (colorData.defaultColor) {
                    result = colorData;
                }
            }

            return result;
        }

        public void SetColor(int i) {
            currentColor = colors[i];
            SetColor(currentColor);
        }
    }
}