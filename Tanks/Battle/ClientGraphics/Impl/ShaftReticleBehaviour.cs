using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftReticleBehaviour : MonoBehaviour {
        [SerializeField] float maxSize = 512f;

        new RectTransform transform;

        public void Init() => transform = gameObject.GetComponent<RectTransform>();

        public void UpdateSize() {
            float b = Mathf.Min(Screen.width, Screen.height);
            float num = Mathf.Min(maxSize, b);
            transform.sizeDelta = new Vector2(num, num);
        }
    }
}