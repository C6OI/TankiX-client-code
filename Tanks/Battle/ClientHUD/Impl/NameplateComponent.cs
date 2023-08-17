using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(CanvasGroup))]
    public class NameplateComponent : MonoBehaviour, Component {
        static readonly float TEAM_NAMEPLATE_Y_OFFSET = 1.8f;

        public float yOffset = 2f;

        public float nickOffset;

        public float appearanceSpeed = 0.2f;

        public float disappearanceSpeed = 0.2f;

        public bool alwaysVisible;

        [SerializeField] EntityBehaviour redHealthBarPrefab;

        [SerializeField] EntityBehaviour blueHealthBarPrefab;

        [SerializeField] Graphic colorProvider;

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

        public void AddRedHealthBar(Entity entity) => AddHealthBar(redHealthBarPrefab).BuildEntity(entity);

        public void AddBlueHealthBar(Entity entity) => AddHealthBar(blueHealthBarPrefab).BuildEntity(entity);

        EntityBehaviour AddHealthBar(EntityBehaviour prefab) {
            EntityBehaviour entityBehaviour = Instantiate(prefab);
            entityBehaviour.transform.SetParent(transform, false);
            yOffset = TEAM_NAMEPLATE_Y_OFFSET;
            return entityBehaviour;
        }
    }
}