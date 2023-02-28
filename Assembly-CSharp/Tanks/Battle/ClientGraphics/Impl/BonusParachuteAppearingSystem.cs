using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BonusParachuteAppearingSystem : ECSSystem {
        [OnEventFire]
        public void ResetBonusParachuteAlpha(NodeAddedEvent e, BonusParachuteSpawnAppearingNode node) {
            node.parachuteMaterial.Material.SetFullTransparent();
        }

        [OnEventFire]
        public void UpdateBonusParachuteAlpha(TimeUpdateEvent e, BonusParachuteSpawnAppearingNode node) {
            float progress = Date.Now.GetProgress(node.bonusDropTime.DropTime, node.bonusConfig.SpawnDuration);
            node.parachuteMaterial.Material.SetAlpha(progress);
        }

        [OnEventFire]
        public void SetFullOpacity(NodeRemoveEvent e, BonusParachuteSpawnAppearingNode node) {
            node.parachuteMaterial.Material.SetFullOpacity();
        }

        [OnEventFire]
        public void FoldParachute(BonusTakenEvent e, BonusWithParachuteNode bonus) {
            bonus.Entity.AddComponent<BonusParachuteFoldingStateComponent>();
        }

        [OnEventFire]
        public void SeparateFoldingParachute(NodeAddedEvent e, AttachedFoldingParachuteNode bonusWithParachute, [JoinAll] SingleNode<BonusClientConfigComponent> bonusConfig) {
            Entity entity = CreateEntity("separateParachute");
            SeparateParachuteComponent separateParachuteComponent = new();
            separateParachuteComponent.parachuteFoldingScaleByXZ = bonusConfig.component.parachuteFoldingScaleByXZ;
            separateParachuteComponent.parachuteFoldingScaleByY = bonusConfig.component.parachuteFoldingScaleByY;
            SeparateParachuteComponent component = separateParachuteComponent;
            entity.AddComponent(component);
            entity.AddComponent(new BonusParachuteInstanceComponent(bonusWithParachute.bonusParachuteInstance.BonusParachuteInstance));
            entity.AddComponent(new LocalDurationComponent(bonusConfig.component.parachuteFoldingDuration));
            bonusWithParachute.Entity.RemoveComponent<BonusParachuteInstanceComponent>();
            bonusWithParachute.Entity.RemoveComponent<TopParachuteMarkerComponent>();
            bonusWithParachute.Entity.RemoveComponent<BonusParachuteFoldingStateComponent>();
        }

        public class BonusParachuteSpawnAppearingNode : Node {
            public BonusComponent bonus;

            public BonusConfigComponent bonusConfig;

            public BonusDropTimeComponent bonusDropTime;

            public BonusParachuteInstanceComponent bonusParachuteInstance;
            public BonusSpawnStateComponent bonusSpawnState;

            public ParachuteMaterialComponent parachuteMaterial;
        }

        public class BonusWithParachuteNode : Node {
            public BonusBoxInstanceComponent bonusBoxInstance;

            public BonusParachuteInstanceComponent bonusParachuteInstance;
        }

        public class AttachedFoldingParachuteNode : Node {
            public BonusParachuteFoldingStateComponent bonusParachuteFoldingState;

            public BonusParachuteInstanceComponent bonusParachuteInstance;
        }
    }
}