using System.Collections.Generic;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AnimatedPaintComponent : MonoBehaviour {
        [SerializeField] float saturtion = 1f;

        [SerializeField] float value = 1f;

        [SerializeField] float speed = 1f;

        float hue = 1f;

        readonly List<Material> materials = new();

        void Start() {
            hue = Random.Range(0f, 1f);
        }

        void Update() {
            Color color = Color.HSVToRGB(hue, saturtion, value);
            hue += Time.deltaTime * speed;

            if (hue > 1f) {
                hue = 0f;
            }

            foreach (Material material in materials) {
                material.SetColor(TankMaterialPropertyNames.COLORING_ID, color);
            }
        }

        public void AddMaterial(Material material) {
            materials.Add(material);
        }
    }
}