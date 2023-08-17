using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BonusBoxDisappearingSystem : ECSSystem {
        [OnEventFire]
        public void UpdateBonusBoxAlpha(TimeUpdateEvent e, TakenBonusBoxNode node) {
            float progress = Date.Now.GetProgress(node.localDuration.StartedTime, node.localDuration.Duration);
            float alpha = 1f - progress;
            node.material.Material.SetAlpha(alpha);
        }

        [OnEventFire]
        public void UpdateBrokenBonusBoxAlpha(TimeUpdateEvent e, TakenBrokenBonusBoxNode node) {
            float progress = Date.Now.GetProgress(node.localDuration.StartedTime, node.localDuration.Duration);
            float num = !(progress < 0.9f) ? (progress - 0.9f) / 0.1f : 0f;
            float alpha = 1f - num;
            node.materialArray.Materials.SetAlpha(alpha);
        }

        [OnEventFire]
        public void RemoveBrokenBox(LocalDurationExpireEvent e, TakenBrokenBonusBoxNode bonus) {
            Object.Destroy(bonus.brokenBonusBoxInstance.Instance);
            DeleteEntity(bonus.Entity);
        }

        public class TakenBonusBoxNode : Node {
            public BonusBoxInstanceComponent bonusBoxInstance;
            public BonusTakingStateComponent bonusTakingState;

            public LocalDurationComponent localDuration;

            public MaterialComponent material;
        }

        public class TakenBrokenBonusBoxNode : Node {
            public BonusTakingStateComponent bonusTakingState;

            public BrokenBonusBoxInstanceComponent brokenBonusBoxInstance;

            public LocalDurationComponent localDuration;

            public MaterialArrayComponent materialArray;
        }
    }
}