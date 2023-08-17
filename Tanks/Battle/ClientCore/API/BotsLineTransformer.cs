using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientCore.API {
    public class BotsLineTransformer : MonoBehaviour {
        public Vector3 direction = new(0f, 0f, 1f);

        public float gapBetweenBots = 5f;

        public void TransformBots() {
            PhysicalRootBehaviour[] array = FindObjectsOfType<PhysicalRootBehaviour>();
            array[0].transform.position = this.transform.position;

            for (int i = 0; i < array.Length; i++) {
                Transform transform = array[i].gameObject.transform;
                transform.position = this.transform.position + i * gapBetweenBots * direction;
                transform.rotation = this.transform.rotation;
            }
        }
    }
}