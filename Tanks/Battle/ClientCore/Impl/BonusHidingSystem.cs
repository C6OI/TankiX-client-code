using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class BonusHidingSystem : ECSSystem {
        public const float HIDING_DURATION = 1f;

        [OnEventComplete]
        public void HideParachute(HideBonusEvent e, SingleNode<BonusParachuteInstanceComponent> bonusParachute) {
            UnityUtil.Destroy(bonusParachute.component.BonusParachuteInstance);
            bonusParachute.Entity.RemoveComponent<BonusParachuteInstanceComponent>();
        }

        [OnEventFire]
        public void CreateBonusHiddingEffectEntity(HideBonusEvent e, BonusBoxNode bonusBox) {
            Entity entity = CreateEntity("BonusHiding");
            entity.AddComponent<BonusRoundEndStateComponent>();
            BonusBoxInstanceComponent bonusBoxInstanceComponent = new();
            bonusBoxInstanceComponent.BonusBoxInstance = bonusBox.bonusBoxInstance.BonusBoxInstance;
            entity.AddComponent(bonusBoxInstanceComponent);
            bonusBox.bonusBoxInstance.Removed = true;
            entity.AddComponent(new LocalDurationComponent(1f));
        }

        [OnEventFire]
        public void HidingProcess(UpdateEvent e, BonusBoxHidingNode bonus) {
            float progress = Date.Now.GetProgress(bonus.localDuration.StartedTime, 1f);
            float num = 1f - progress;
            bonus.bonusBoxInstance.BonusBoxInstance.transform.localScale = new Vector3(num, num, num);
        }

        [OnEventFire]
        public void DestroyBonusBox(LocalDurationExpireEvent e, BonusBoxHidingNode hiddingBonus) {
            UnityUtil.Destroy(hiddingBonus.bonusBoxInstance.BonusBoxInstance);
            DeleteEntity(hiddingBonus.Entity);
        }

        public class BonusBoxNode : Node {
            public BattleGroupComponent battleGroup;
            public BonusBoxInstanceComponent bonusBoxInstance;
        }

        public class BonusBoxHidingNode : Node {
            public BonusBoxInstanceComponent bonusBoxInstance;

            public BonusRoundEndStateComponent bonusRoundEndState;
            public LocalDurationComponent localDuration;
        }
    }
}