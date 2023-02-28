using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class CustomRenderQueue {
        public const int BONUS_REGION_RENDER_QUEUE = 3100;

        public const int WEAPON_EFFECT_RENDER_QUEUE = 3150;

        public const int DECAL_BASE_RENDER_QUEUE = 2200;

        public const int SHAFT_AIMING_RENDER_QUEUE = 3500;

        public const int WATER_RENDER_QUEUE = 2451;

        public static void SetQueue(GameObject gameObject, int queue) {
            Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
            Renderer[] array = componentsInChildren;

            foreach (Renderer renderer in array) {
                renderer.material.renderQueue = queue;
            }
        }
    }
}