using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(CanvasGroup))]
    public class NameplateComponent : BehaviourComponent {
        const float TEAM_NAMEPLATE_Y_OFFSET = 1.2f;

        public float yOffset = 2f;

        public float appearanceSpeed = 0.2f;

        public float disappearanceSpeed = 0.2f;

        public bool alwaysVisible;

        [SerializeField] EntityBehaviour redHealthBarPrefab;

        [SerializeField] EntityBehaviour blueHealthBarPrefab;

        [SerializeField] Graphic colorProvider;

        Camera _cachedCamera;

        CanvasGroup canvasGroup;

        public Color Color {
            get => colorProvider.color;
            set => colorProvider.color = value;
        }

        CanvasGroup CanvasGroup {
            get {
                if (canvasGroup == null) {
                    canvasGroup = GetComponent<CanvasGroup>();
                }

                return canvasGroup;
            }
        }

        public float Alpha {
            get => CanvasGroup.alpha;
            set => CanvasGroup.alpha = value;
        }

        public Camera CachedCamera {
            get {
                if (!_cachedCamera) {
                    _cachedCamera = Camera.main;
                }

                return _cachedCamera;
            }
        }

        public void AddRedHealthBar(Entity entity) {
            AddHealthBar(redHealthBarPrefab).BuildEntity(entity);
        }

        public void AddBlueHealthBar(Entity entity) {
            AddHealthBar(blueHealthBarPrefab).BuildEntity(entity);
        }

        EntityBehaviour AddHealthBar(EntityBehaviour prefab) {
            EntityBehaviour entityBehaviour = Instantiate(prefab);
            entityBehaviour.transform.SetParent(transform, false);
            yOffset = 1.2f;
            return entityBehaviour;
        }
    }
}