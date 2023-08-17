using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftReticleSpotBehaviour : MonoBehaviour {
        [SerializeField] float minSpotScale = 1f;

        [SerializeField] float maxSpotScale = 8f;

        [SerializeField] float minSpotDistance = 50f;

        [SerializeField] float maxSpotDistance = 5000f;

        new RectTransform transform;

        public void Init() => transform = gameObject.GetComponent<RectTransform>();

        public void SetSizeByDistance(float distance, bool isInsideTankPart) {
            float num = CalculateScaleByDistance(distance, isInsideTankPart);
            transform.localScale = new Vector3(num, num, 1f);
        }

        public void SetDefaultSize() => transform.localScale = Vector3.one;

        float CalculateScaleByDistance(float distance, bool isInsideTankPart) {
            if (isInsideTankPart) {
                return maxSpotScale;
            }

            if (distance < minSpotDistance) {
                return maxSpotScale;
            }

            if (distance > maxSpotDistance) {
                return minSpotScale;
            }

            float num = distance - minSpotDistance;
            return (1f - num / (maxSpotDistance - minSpotDistance)) * (maxSpotScale - minSpotScale) + minSpotScale;
        }
    }
}