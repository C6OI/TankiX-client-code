using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Assets {
    public class EffectsContainerComponent : BehaviourComponent {
        [SerializeField] RectTransform buffContainer;

        [SerializeField] RectTransform debuffContainer;

        [SerializeField] EntityBehaviour effectPrefab;

        public void SpawnBuff(Entity entity) {
            SpawnEffect(buffContainer, entity);
        }

        public void SpawnDebuff(Entity entity) {
            SpawnEffect(debuffContainer, entity);
        }

        void SpawnEffect(RectTransform container, Entity entity) {
            EntityBehaviour entityBehaviour = Instantiate(effectPrefab);
            entityBehaviour.BuildEntity(entity);
            entityBehaviour.transform.SetParent(container, false);
            entityBehaviour.transform.SetAsFirstSibling();
            SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
        }
    }
}