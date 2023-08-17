using UnityEngine;
using UnityStandardAssets.Water;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientGraphics.API {
    public class WaterComponent : MonoBehaviour, Component {
        public void DisableReflection() => DisableReflection(transform);

        void DisableReflection(Transform root) {
            WaterTile component = root.GetComponent<WaterTile>();

            if (component != null) {
                component.reflection.enabled = false;
                component.enabled = false;
            }

            for (int i = 0; i < root.childCount; i++) {
                DisableReflection(root.GetChild(i));
            }
        }
    }
}