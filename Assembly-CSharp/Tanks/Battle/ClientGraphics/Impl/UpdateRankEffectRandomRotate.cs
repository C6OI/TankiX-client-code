using System.Collections;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateRankEffectRandomRotate : MonoBehaviour {
        public bool isRotate = true;

        public int fps = 30;

        public int x = 100;

        public int y = 200;

        public int z = 300;

        float deltaTime;

        bool isVisible;

        float rangeX;

        float rangeY;

        float rangeZ;

        void Start() {
            deltaTime = 1f / fps;
            rangeX = Random.Range(0, 10);
            rangeY = Random.Range(0, 10);
            rangeZ = Random.Range(0, 10);
        }

        void OnBecameInvisible() {
            isVisible = false;
        }

        void OnBecameVisible() {
            isVisible = true;
            StartCoroutine(UpdateRotation());
        }

        IEnumerator UpdateRotation() {
            while (isVisible) {
                if (isRotate) {
                    transform.Rotate(deltaTime * Mathf.Sin(Time.time + rangeX) * x, deltaTime * Mathf.Sin(Time.time + rangeY) * y, deltaTime * Mathf.Sin(Time.time + rangeZ) * z);
                }

                yield return new WaitForSeconds(deltaTime);
            }
        }
    }
}