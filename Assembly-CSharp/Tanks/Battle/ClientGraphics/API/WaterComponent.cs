using UnityEngine;
using UnityStandardAssets.Water;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class WaterComponent : MonoBehaviour, Component {
        public bool EdgeBlend {
            set => GetComponent<WaterBase>().edgeBlend = value;
        }

        void OnEnable() {
            SetWaterRenderQueue(transform);
        }

        public void DisableReflection() {
            DisableReflection(transform);
        }

        void DisableReflection(Transform root) {
            WaterTile component = root.GetComponent<WaterTile>();

            if (component != null) {
                component.reflection.enabled = false;
            }

            for (int i = 0; i < root.childCount; i++) {
                DisableReflection(root.GetChild(i));
            }
        }

        void SetWaterRenderQueue(Transform root) {
            WaterTile component = root.GetComponent<WaterTile>();

            if (component != null) {
                Material[] materials = root.GetComponent<Renderer>().materials;

                for (int i = 0; i < materials.Length; i++) {
                    materials[i].renderQueue = 2451;
                }
            }

            for (int j = 0; j < root.childCount; j++) {
                SetWaterRenderQueue(root.GetChild(j));
            }
        }
    }
}