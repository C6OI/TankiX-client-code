using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public class ParallaxContainer : MonoBehaviour {
        [SerializeField] bool isActive = true;

        [SerializeField] Camera camera;

        [SerializeField] RectTransform layer;

        [SerializeField] RectTransform background;

        [SerializeField] RectTransform container;

        [SerializeField] float mainTransformRotateFactor = 10f;

        [SerializeField] float layerMoveFactor = 10f;

        [SerializeField] float backgroundMoveFactor = 20f;

        [SerializeField] float lerpSpeed = 20f;

        Vector3 mousePos;

        public bool IsActive {
            get => isActive;
            set => isActive = value;
        }

        void Update() {
            if (!isActive) {
                if ((bool)container && transform.eulerAngles.x != 0f || transform.eulerAngles.y != 0f) {
                    transform.eulerAngles = new Vector3(Mathf.MoveTowardsAngle(transform.eulerAngles.x, 0f, Time.deltaTime * mainTransformRotateFactor * 10f),
                        Mathf.MoveTowardsAngle(transform.eulerAngles.y, 0f, Time.deltaTime * mainTransformRotateFactor * 10f));
                }

                return;
            }

            float num = !container ? Screen.height : container.rect.height;
            float num2 = !container ? Screen.width : container.rect.width;
            Vector3 vector = camera.WorldToScreenPoint(transform.position);
            mousePos = Vector2.Lerp(mousePos, !isActive ? vector : Input.mousePosition, Time.deltaTime * lerpSpeed);
            float num3 = Mathf.Clamp((mousePos.x - vector.x) / num2, -1f, 1f);
            float num4 = Mathf.Clamp((mousePos.y - vector.y) / num, -1f, 1f);
            num3 = !(num3 > 0f) ? Mathf.Max(num3, -1f) : Mathf.Min(num3, 1f);
            num4 = !(num4 > 0f) ? Mathf.Max(num4, -1f) : Mathf.Min(num4, 1f);
            transform.eulerAngles = new Vector3(num4 * mainTransformRotateFactor, (0f - num3) * mainTransformRotateFactor);

            if ((bool)layer) {
                layer.anchoredPosition = new Vector2((0f - num3) * layerMoveFactor + 2.6f, (0f - num4) * layerMoveFactor);
            }

            if ((bool)background) {
                background.anchoredPosition = new Vector2((0f - num3) * backgroundMoveFactor, (0f - num4) * backgroundMoveFactor);
            }
        }

        void OnEnable() {
            camera = GetComponentInParent<Canvas>().worldCamera;
            mousePos = camera.WorldToScreenPoint(transform.position);
        }

        [ContextMenu("Reset layers")]
        public void ResetLayers() { }
    }
}