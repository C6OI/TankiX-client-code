using System;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Serializable]
    public class ColorData {
        [SerializeField] public Color color = Color.white;

        [SerializeField] public Color hardlightColor = Color.green;

        [SerializeField] public Material material;

        [SerializeField] public bool defaultColor;

        public Color Color {
            get => color;
            set => color = value;
        }

        public Color HardlightColor {
            get => hardlightColor;
            set => hardlightColor = value;
        }

        public Material Material {
            get => material;
            set => material = value;
        }

        public bool DefaultColor {
            get => defaultColor;
            set => defaultColor = value;
        }
    }
}