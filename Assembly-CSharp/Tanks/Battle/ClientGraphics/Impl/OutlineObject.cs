using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class OutlineObject : MonoBehaviour {
        [SerializeField] Color glowColor;

        [Range(0f, 1f)] public float saturation;

        public float LerpFactor = 10f;

        public bool Enable;

        Color _currentColor;

        readonly List<Material> _materials = new();

        Color _targetColor;

        public Color GlowColor {
            get => glowColor;
            set => glowColor = value;
        }

        public Renderer[] Renderers { get; private set; }

        void Start() {
            Renderers = GetComponentsInChildren<Renderer>();
            Renderer[] renderers = Renderers;

            foreach (Renderer renderer in renderers) {
                _materials.AddRange(renderer.materials);
            }
        }

        void Update() {
            if (Enable) {
                _targetColor = glowColor * saturation;
            } else {
                _targetColor = Color.black;
            }

            _currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

            for (int i = 0; i < _materials.Count; i++) {
                _materials[i].SetColor("_outlineColor", _currentColor);
            }
        }
    }
}