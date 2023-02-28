using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BonusBoxAppearingSystem : ECSSystem {
        [OnEventFire]
        public void SetFullTransparent(NodeAddedEvent e, BonusBoxSpawnAppearingNode node) {
            node.material.Material.SetFullTransparent();
        }

        [OnEventFire]
        public void UpdateBonusBoxAlpha(TimeUpdateEvent e, BonusBoxSpawnAppearingNode node) {
            float progress = Date.Now.GetProgress(node.bonusDropTime.DropTime, node.bonusConfig.SpawnDuration);
            node.material.Material.SetAlpha(progress);
        }

        [OnEventFire]
        public void SetFullOpacity(NodeRemoveEvent e, BonusBoxSpawnAppearingNode node) {
            node.material.Material.SetFullOpacity();
        }

        [OnEventFire]
        public void CreateBrokenBonusBox(BonusTakenEvent e, BonusWithResourceNode bonus, [JoinAll] SingleNode<BonusClientConfigComponent> bonusConfig) {
            GameObject brokenBonusGameObject = bonus.bonusBoxInstance.BonusBoxInstance.GetComponent<BrokenBonusBoxBehavior>().BrokenBonusGameObject;
            Transform transform = bonus.bonusBoxInstance.BonusBoxInstance.transform;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = brokenBonusGameObject;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, bonus);
            Transform instance = getInstanceFromPoolEvent2.Instance;
            GameObject gameObject = instance.gameObject;
            instance.position = transform.position;
            instance.rotation = transform.rotation;
            Material[] allMaterials = MaterialAlphaUtils.GetAllMaterials(gameObject);
            allMaterials.SetOverrideTag("RenderType", "Transparent");
            Entity entity = CreateEntity("brokenBonusBox");
            entity.AddComponent(new MaterialArrayComponent(allMaterials));
            entity.AddComponent(new BrokenBonusBoxInstanceComponent(gameObject));
            entity.AddComponent<BonusTakingStateComponent>();
            entity.AddComponent(new LocalDurationComponent(bonusConfig.component.disappearingDuration));
            gameObject.SetActive(true);
        }

        public class BonusBoxSpawnAppearingNode : Node {
            public BonusComponent bonus;

            public BonusBoxInstanceComponent bonusBoxInstance;

            public BonusConfigComponent bonusConfig;

            public BonusDropTimeComponent bonusDropTime;
            public BonusSpawnStateComponent bonusSpawnState;

            public MaterialComponent material;
        }

        public class BonusWithResourceNode : Node {
            public BonusBoxInstanceComponent bonusBoxInstance;
        }
    }
}